
using Allocation.Allot.Repositories;

namespace Allocation.Allot.Endpoints
{
    using OfficeOpenXml;
    using Serenity;
    using Serenity.Data;
    using Serenity.Services;
    using Serenity.Web;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Mvc;
    using MyRow = Entities.DeclarationDataRow;

    [RoutePrefix("Services/Allot/DeclarationDataImport"), Route("{action}")]
    [ConnectionKey(typeof(MyRow)), ServiceAuthorize]
    public class DeclarationDataExcelImportController : ServiceEndpoint
    {
        [HttpPost]
        public ExcelImportResponse ExcelImport(IUnitOfWork uow, ExcelImportRequest request)
        {
            request.CheckNotNull();
            Check.NotNullOrWhiteSpace(request.FileName, "filename");

            UploadHelper.CheckFileNameSecurity(request.FileName);

            if (!request.FileName.StartsWith("temporary/"))
                throw new ArgumentOutOfRangeException("filename");

            if (Path.GetExtension(request.FileName) != ".xlsx")
            {
                throw new Exception("文件格式不对，只支持Excel2007及以上版本");
            }

            ExcelPackage ep = new ExcelPackage();
            using (var fs = new FileStream(UploadHelper.DbFilePath(request.FileName), FileMode.Open, FileAccess.Read))
            {
                ep.Load(fs);
            }

            var p = MyRow.Fields;

            var response = new ExcelImportResponse();
            response.ErrorList = new List<string>();

            var worksheet = ep.Workbook.Worksheets[1];
            bool isCreat = false;
            var user = (UserDefinition)Authorization.UserDefinition;
            for (var row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                try
                {
                    // every circulation should reset the flag
                    isCreat = false;
                    // check the productno is empty or not
                    var masterNo = worksheet.Cells[row, 2].GetValue<string>();
                    var no = worksheet.Cells[row, 3].GetValue<string>();

                    if (masterNo.IsTrimmedEmpty() || no.IsTrimmedEmpty())
                        continue;
                    // search the productno whether existes in the database
                    no = no.Trim();
                    masterNo = masterNo.Trim();
                    var yundan = uow.Connection.TryFirst<MyRow>(q => q
                        .Select(p.SubAwb, p.MasterAwb, p.Id)
                        .Where(p.SubAwb == no && p.MasterAwb == masterNo));

                    if (yundan == null)
                    {
                        yundan = new MyRow
                        {
                            SubAwb = no,
                            MasterAwb = masterNo
                        };

                        isCreat = true;
                    }
                    else
                    {
                        isCreat = false;
                    }
                    //yundan.ApplicationUnit = worksheet.Cells[row, 1].GetValue<string>();
                    yundan.Flight = worksheet.Cells[row, 1].GetValue<string>();
                    int.TryParse(worksheet.Cells[row, 4].GetValue<string>(), out int amount);
                    yundan.Amount = amount;
                    double.TryParse(worksheet.Cells[row, 5].GetValue<string>(), out double weight);
                    yundan.Weight = weight;
                    yundan.Description = worksheet.Cells[row, 6].GetValue<string>();
                    yundan.IsChecked = Entities.StateKind.NoChecked;
                    //yundan.TenantId = user.TenantId;
                    if (isCreat)
                    {
                        new DeclarationDataRepository().Create(uow, new SaveRequest<MyRow>
                        {
                            Entity = yundan
                        });

                        response.Inserted = response.Inserted + 1;
                    }
                    else
                    {
                        new DeclarationDataRepository().Update(uow, new SaveRequest<MyRow>
                        {
                            Entity = yundan,
                            EntityId = yundan.Id
                        });
                        response.Updated = response.Updated + 1;
                    }
                }
                catch (Exception ex)
                {
                    ex.Log();
                    response.ErrorList.Add("Exception on Row " + row + ": " + ex.Message);
                }
            }

            return response;
        }
    }
}

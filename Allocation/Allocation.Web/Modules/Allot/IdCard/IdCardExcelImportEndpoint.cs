using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using OfficeOpenXml;
using Serenity;
using Serenity.Data;
using Serenity.Services;
using Serenity.Web;
using MyRow = Allocation.Allot.Entities.IdCardRow;
namespace Allocation.Allot.Endpoints
{
    [Route("Services/Allot/IdCardExcelImport/[action]")]
    [ConnectionKey(typeof(MyRow)), ServiceAuthorize]
    public class IdCardExcelImportEndpoint: ServiceEndpoint
    {
        [HttpPost]
        public ExcelImportResponse ExcelImport(IUnitOfWork uow, ExcelImportRequest request)
        {
            request.CheckNotNull();
            Check.NotNullOrWhiteSpace(request.FileName, "filename");

            UploadHelper.CheckFileNameSecurity(request.FileName);

            if (!request.FileName.StartsWith("temporary/"))
                throw new ArgumentOutOfRangeException("filename");

            ExcelPackage ep = new ExcelPackage();
            using (var fs = new FileStream(UploadHelper.DbFilePath(request.FileName), FileMode.Open, FileAccess.Read))
                ep.Load(fs);

            var p = MyRow.Fields;


            var response = new ExcelImportResponse { ErrorList = new List<string>() };

            if (ep.Workbook.Worksheets.Count == 0)
            {
                response.ErrorList.Add("The Excel file doesn't cantain any sheet");
                return response;
            }
            var orderSheet = ep.Workbook.Worksheets[1];

            //先遍历获取所有的订单明细信息
            var idCards = new List<MyRow>();
            for (var row = 2; row <= orderSheet.Dimension.End.Row; row++)
            {
                var name = Convert.ToString(orderSheet.Cells[row, 1].Value ?? string.Empty);
                var cardNo = Convert.ToString(orderSheet.Cells[row, 2].Value ?? string.Empty);
                if (name.IsTrimmedEmpty()|| cardNo.IsTrimmedEmpty())
                {
                    continue;
                }
                var tmpRow = new MyRow();
                tmpRow.Name = name;
                tmpRow.CardNo = cardNo;

                idCards.Add(tmpRow);
            }


            for (var row = 2; row <= orderSheet.Dimension.End.Row; row++)
            {
                try
                {

                    response.Inserted = response.Inserted + 1;
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
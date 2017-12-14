
using System;
using System.IO;
using System.Text;
using Allocation.Modules.Common;
using Serenity.Reporting;
using Serenity.Web;

namespace Allocation.Allot.Endpoints
{
    using Serenity;
    using Serenity.Data;
    using Serenity.Services;
    using System.Data;
    using System.Web.Mvc;
    using MyRepository = Repositories.DeclarationDataRepository;
    using MyRow = Entities.DeclarationDataRow;

    [RoutePrefix("Services/Allot/DeclarationData"), Route("{action}")]
    [ConnectionKey(typeof(MyRow)), ServiceAuthorize(typeof(MyRow))]
    public class DeclarationDataController : ServiceEndpoint
    {
        [HttpPost, AuthorizeCreate(typeof(MyRow))]
        public SaveResponse Create(IUnitOfWork uow, SaveRequest<MyRow> request)
        {
            return new MyRepository().Create(uow, request);
        }

        [HttpPost, AuthorizeUpdate(typeof(MyRow))]
        public SaveResponse Update(IUnitOfWork uow, SaveRequest<MyRow> request)
        {
            return new MyRepository().Update(uow, request);
        }

        [HttpPost, AuthorizeDelete(typeof(MyRow))]
        public DeleteResponse Delete(IUnitOfWork uow, DeleteRequest request)
        {
            return new MyRepository().Delete(uow, request);
        }

        [HttpPost]
        public RetrieveResponse<MyRow> Retrieve(IDbConnection connection, RetrieveRequest request)
        {
            return new MyRepository().Retrieve(connection, request);
        }

        [HttpPost]
        public ListResponse<MyRow> List(IDbConnection connection, ListRequest request)
        {
            return new MyRepository().List(connection, request);
        }

        public FileContentResult ListExcel(IDbConnection connection, ListRequest request)
        {
            var data = List(connection, request).Entities;
            var report = new DynamicDataReport(data, request.IncludeColumns, typeof(Columns.DeclarationDataColumns));
            var bytes = new ReportRepository().Render(report);
            return ExcelContentResult.Create(bytes, "DeclarationData_" +
                                                    DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx");
        }

        public FileContentResult ExcelTemplate(IDbConnection connection, ListRequest request)
        {
            FileInfo file = new FileInfo(Server.MapPath("~/ExcelTemplate/Template.xlsx"));
            using (FileStream fs = file.OpenRead())
            {
                BinaryReader r = new BinaryReader(fs);
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                byte[] bytes = r.ReadBytes((int)r.BaseStream.Length);
                return ExcelContentResult.Create(bytes, "Template.xlsx");
            }
        }

        [HttpPost, AuthorizeDelete(typeof(MyRow))]
        public DeleteResponse BatchDelete(IUnitOfWork uow, BatchDeleteRequest request)
        {
            StringBuilder sb = new StringBuilder();
            var user = (UserDefinition)Authorization.UserDefinition;
            foreach (var id in request.EntityIds)
            {
                if (Authorization.HasPermission(Administration.PermissionKeys.Tenants))
                {
                    sb.Append($"delete from [allot].[DeclarationData] where Id={id}");
                }
                else
                {
                    sb.Append($"delete from [allot].[DeclarationData] where Id={id} and TenantId={user.TenantId};");
                }
            }
            if (sb.Length > 0)
            {
                uow.Connection.Execute(sb.ToString());
            }
            return new DeleteResponse();
        }
    }
}

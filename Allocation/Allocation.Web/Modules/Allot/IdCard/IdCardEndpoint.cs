
namespace Allocation.Allot.Endpoints
{
    using Serenity;
    using Serenity.Data;
    using Serenity.Reporting;
    using Serenity.Services;
    using Serenity.Web;
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using MyRepository = Repositories.IdCardRepository;
    using MyRow = Entities.IdCardRow;

    [RoutePrefix("Services/Allot/IdCard"), Route("{action}")]
    [ConnectionKey(typeof(MyRow)), ServiceAuthorize(typeof(MyRow))]
    public class IdCardController : ServiceEndpoint
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
            var cols = request.IncludeColumns.Where(p => p != MyRow.Fields.ReusedCount.Name);
            var report = new DynamicDataReport(data, cols, typeof(Columns.IdCardColumns));
            var bytes = new ReportRepository().Render(report);
            return ExcelContentResult.Create(bytes, "IdCard_" +
                                                    DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx");
        }

        public FileContentResult ExcelTemplate(IDbConnection connection, ListRequest request)
        {
            FileInfo file = new FileInfo(Server.MapPath("~/ExcelTemplate/IdCardTemplate.xlsx"));
            using (FileStream fs = file.OpenRead())
            {
                BinaryReader r = new BinaryReader(fs);
                r.BaseStream.Seek(0, SeekOrigin.Begin); //将文件指针设置到文件开
                byte[] bytes = r.ReadBytes((int)r.BaseStream.Length);
                return ExcelContentResult.Create(bytes, "IdCardTemplate.xlsx");
            }
        }
    }
}

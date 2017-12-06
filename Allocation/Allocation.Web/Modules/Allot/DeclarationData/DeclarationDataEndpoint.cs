
using System.Text;
using Allocation.Modules.Common;

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

        [HttpPost, AuthorizeDelete(typeof(MyRow))]
        public DeleteResponse BatchDelete(IUnitOfWork uow, BatchDeleteRequest request)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var id in request.EntityIds)
            {
                sb.Append($"delete from [allot].[DeclarationData] where Id={id};");
            }
            if (sb.Length > 0)
            {
                uow.Connection.Execute(sb.ToString());
            }
            return new DeleteResponse();
        }
    }
}

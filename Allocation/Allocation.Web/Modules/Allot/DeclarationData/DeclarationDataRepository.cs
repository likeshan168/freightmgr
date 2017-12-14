
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Allocation.Modules.Common;

namespace Allocation.Allot.Repositories
{
    using Serenity;
    using Serenity.Data;
    using Serenity.Services;
    using System;
    using System.Data;
    using MyRow = Entities.DeclarationDataRow;

    public class DeclarationDataRepository
    {
        private static MyRow.RowFields fld { get { return MyRow.Fields; } }

        public SaveResponse Create(IUnitOfWork uow, SaveRequest<MyRow> request)
        {
            return new MySaveHandler().Process(uow, request, SaveRequestType.Create);
        }

        public SaveResponse Update(IUnitOfWork uow, SaveRequest<MyRow> request)
        {
            return new MySaveHandler().Process(uow, request, SaveRequestType.Update);
        }

        public DeleteResponse Delete(IUnitOfWork uow, DeleteRequest request)
        {
            return new MyDeleteHandler().Process(uow, request);
        }

        public RetrieveResponse<MyRow> Retrieve(IDbConnection connection, RetrieveRequest request)
        {
            return new MyRetrieveHandler().Process(connection, request);
        }

        public ListResponse<MyRow> List(IDbConnection connection, ListRequest request)
        {
            return new MyListHandler().Process(connection, request);
        }

        public IEnumerable<MyRow> GetList(ListRequest request)
        {
            using (var connection = SqlConnections.NewFor<MyRow>())
            {
                return new MyListHandler().Process(connection, request).Entities;
            }
        }

        public AppResponse Update(IList<MyRow> rows)
        {
            AppResponse loginResponse = new AppResponse();

            if (rows.Any())
            {
                var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                var trans = connection.BeginTransaction();
                try
                {
                    #region will be removed
                    //using (var connection = SqlConnections.NewFor<MyRow>())
                    //{
                    //var uow = new UnitOfWork(connection);

                    //foreach (var row in rows)
                    //{
                    //    if (row.Id.HasValue)
                    //    {
                    //        new MySaveHandler().Process(uow, new SaveRequest<MyRow>
                    //        {
                    //            Entity = row,
                    //            EntityId = row.Id
                    //        }, SaveRequestType.Update);
                    //    }
                    //    else
                    //    {
                    //        new MySaveHandler().Process(uow, new SaveRequest<MyRow>
                    //        {
                    //            Entity = row,
                    //        }, SaveRequestType.Create);
                    //    }
                    //} 
                    #endregion

                    StringBuilder sb = new StringBuilder();
                    var user = (UserDefinition)Authorization.UserDefinition;
                    //这个是管理员在操作
                    if (Authorization.HasPermission(Administration.PermissionKeys.Tenants))
                    {
                        foreach (var row in rows)
                        {
                            if (row.Id.HasValue && row.Id != 0)
                            {
                                if (row.IsChecked != null)
                                    sb.Append(
                                        $"update [allot].[DeclarationData] set IsChecked={(int)row.IsChecked} where id ={row.Id}");
                            }
                            else
                            {
                                //还要判断是否在网页上进行过添加，如果进行过，那就不需要再进行了
                                if (row.IsChecked != null)
                                    sb.Append(
                                    $"if not exists (select Id from [allot].[DeclarationData] where MasterAwb='{row.MasterAwb}' and SubAwb='{row.SubAwb}') Insert into [allot].[DeclarationData](MasterAwb,SubAwb,Amount, IsChecked) values('{row.MasterAwb}','{row.SubAwb}',{row.Amount},{(int)row.IsChecked});");
                            }
                        }
                    }
                    else
                    {
                        foreach (var row in rows)
                        {
                            if (row.Id.HasValue && row.Id != 0)
                            {
                                if (row.IsChecked != null)
                                    sb.Append(
                                        $"update [allot].[DeclarationData] set IsChecked={(int)row.IsChecked} where id ={row.Id} and TenantId = {user.TenantId};");
                            }
                            else
                            {
                                if (row.IsChecked != null)
                                    sb.Append(
                                        $"if not exists (select Id from [allot].[DeclarationData] where MasterAwb='{row.MasterAwb}' and SubAwb='{row.SubAwb}' and TenantId={user.TenantId}) Insert into [allot].[DeclarationData](MasterAwb,SubAwb,Amount, IsChecked, TenantId) values('{row.MasterAwb}','{row.SubAwb}',{row.Amount},{(int)row.IsChecked} ,{user.TenantId});");
                            }
                        }
                    }
                    var cmd = connection.CreateCommand();
                    cmd.Transaction = trans;
                    cmd.CommandText = sb.ToString();
                    cmd.ExecuteNonQuery();
                    trans.Commit();

                    loginResponse.IsSuccess = true;
                    loginResponse.Result = "数据更新成功";
                    return loginResponse;
                }
                catch (Exception ex)
                {
                    ex.Log();
                    loginResponse.IsSuccess = false;
                    loginResponse.Result = "数据更新失败";
                    trans.Rollback();
                    return loginResponse;
                }
                finally
                {
                    connection.Close();
                }
            }
            loginResponse.IsSuccess = false;
            loginResponse.Result = "更新的不能为空";
            return loginResponse;
        }

        private class MySaveHandler : SaveRequestHandler<MyRow> { }
        private class MyDeleteHandler : DeleteRequestHandler<MyRow> { }
        private class MyRetrieveHandler : RetrieveRequestHandler<MyRow> { }
        private class MyListHandler : ListRequestHandler<MyRow> { }
    }
}
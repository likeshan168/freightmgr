using System;
using Allocation.Administration;
using Allocation.Allot.Entities;
using Serenity;
using Serenity.ComponentModel;
using Serenity.Data;
using Serenity.Web;

namespace Allocation.Modules.Allot.DeclarationData
{
    [LookupScript("Allocation.DeclarationDataMasterAwb")]
    public class DeclarationDataMasterAwbLookup : RowLookupScript<DeclarationDataRow>
    {
        public DeclarationDataMasterAwbLookup()
        {
            Expiration = TimeSpan.FromDays(-1);//可以解决切换不同用户时显示不同的主单号
            IdField = TextField = DeclarationDataRow.Fields.MasterAwb.PropertyName;
        }

        protected override void PrepareQuery(SqlQuery query)
        {
            var fld = DeclarationDataRow.Fields;
            var user = (UserDefinition)Authorization.UserDefinition;
            if (Authorization.HasPermission(PermissionKeys.Tenants))
            {
                query.Distinct(true)
                    //.Select(fld.ApplicationUnit)
                    .Select(fld.MasterAwb)
                    .Where(
                        new Criteria(fld.MasterAwb) != "" &
                        new Criteria(fld.MasterAwb).IsNotNull());
            }
            else
            {
                query.Distinct(true)
                    //.Select(fld.ApplicationUnit)
                    .Select(fld.MasterAwb)
                    .Where(
                        new Criteria(fld.MasterAwb) != "" &
                        new Criteria(fld.MasterAwb).IsNotNull() &
                        new Criteria(fld.TenantId) == user.TenantId);
            }
            
            //new Criteria(fld.ApplicationUnit) != "" &
            //new Criteria(fld.ApplicationUnit).IsNotNull());
        }

        protected override void ApplyOrder(SqlQuery query)
        {
        }
    }
}
using Allocation.Allot.Entities;
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
            IdField = TextField = DeclarationDataRow.Fields.MasterAwb.PropertyName;
        }

        protected override void PrepareQuery(SqlQuery query)
        {
            var fld = DeclarationDataRow.Fields;
            query.Distinct(true)
                .Select(fld.ApplicationUnit)
                .Select(fld.MasterAwb)
                .Where(
                    new Criteria(fld.MasterAwb) != "" &
                    new Criteria(fld.MasterAwb).IsNotNull() &
                    new Criteria(fld.ApplicationUnit) != "" &
                    new Criteria(fld.ApplicationUnit).IsNotNull());
        }

        protected override void ApplyOrder(SqlQuery query)
        {
        }
    }
}
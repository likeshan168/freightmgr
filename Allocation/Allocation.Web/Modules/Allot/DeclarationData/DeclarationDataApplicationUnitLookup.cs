using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Allocation.Allot.Entities;
using Serenity.ComponentModel;
using Serenity.Data;
using Serenity.Web;

namespace Allocation.Modules.Allot.DeclarationData
{
    [LookupScript("Allocation.DeclarationDataApplicationUnit")]
    public class DeclarationDataApplicationUnitLookup: RowLookupScript<DeclarationDataRow>
    {
        public DeclarationDataApplicationUnitLookup()
        {
            IdField = TextField = DeclarationDataRow.Fields.ApplicationUnit.PropertyName;
        }

        protected override void PrepareQuery(SqlQuery query)
        {
            var fld = DeclarationDataRow.Fields;
            query.Distinct(true)
                .Select(fld.ApplicationUnit)
                .Where(
                    new Criteria(fld.ApplicationUnit) != "" &
                    new Criteria(fld.ApplicationUnit).IsNotNull());
        }
    }
}
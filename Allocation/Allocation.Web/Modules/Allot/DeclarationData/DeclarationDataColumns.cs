
using Allocation.Modules.Allot.DeclarationData;

namespace Allocation.Allot.Columns
{
    using Serenity;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.IO;

    [ColumnsScript("Allot.DeclarationData")]
    [BasedOnRow(typeof(Entities.DeclarationDataRow))]
    public class DeclarationDataColumns
    {
        [EditLink, DisplayName("Db.Shared.RecordId"), AlignRight, SortOrder(1)]
        public Int64 Id { get; set; }
        [QuickFilter, IsCheckedFormatter, Width(100)]
        public Boolean IsChecked { get; set; }
        [EditLink]
        [QuickFilter, QuickFilterOption("multiple", true)]
        [LookupEditor(typeof(DeclarationDataApplicationUnitLookup))]
        public String ApplicationUnit { get; set; }

        [QuickFilter, QuickFilterOption("multiple", true), QuickFilterOption("CascadeFrom", "ApplicationUnit")]
        [LookupEditor(typeof(DeclarationDataMasterAwbLookup))]
        public String MasterAwb { get; set; }
        [QuickFilter]
        public String SubAwb { get; set; }
        public Int32 Amount { get; set; }
        public Double Weight { get; set; }
        public String Description { get; set; }
    }
}
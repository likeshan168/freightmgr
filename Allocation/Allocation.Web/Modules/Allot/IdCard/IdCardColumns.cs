
namespace Allocation.Allot.Columns
{
    using Serenity;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.IO;

    [ColumnsScript("Allot.IdCard")]
    [BasedOnRow(typeof(Entities.IdCardRow))]
    public class IdCardColumns
    {
        [EditLink, DisplayName("Db.Shared.RecordId"), AlignRight]
        public Int64 Id { get; set; }
        [Width(100)]
        public String AuthenticationType { get; set; }
        [EditLink, Width(100)]
        public String Name { get; set; }
        [Width(160)]
        public String CardNo { get; set; }
        [Width(100)]
        public String CheckResult { get; set; }
        [Width(140)]
        public DateTime RequestDate { get; set; }
        [Width(120)]
        public String RequestIp { get; set; }
        [Width(80)]
        public Decimal Price { get; set; }
    }
}
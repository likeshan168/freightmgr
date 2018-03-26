
namespace Allocation.Allot.Columns
{
    using Serenity;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.IO;

    [ColumnsScript("Allot.Api")]
    [BasedOnRow(typeof(Entities.ApiRow))]
    public class ApiColumns
    {
        [EditLink, DisplayName("Db.Shared.RecordId"), AlignRight]
        public Int64 Id { get; set; }
        [EditLink, Width(100)]
        public String Title { get; set; }
        [Width(180)]
        public String ApiUrl { get; set; }
        [Width(120)]
        public String UserName { get; set; }
        [Width(120)]
        public String Password { get; set; }
        [Width(160)]
        public String AppKey { get; set; }
        [Width(80)]
        public Decimal Price { get; set; }
        [Width(60)]
        public Boolean IsEnabled { get; set; }
        [Width(140)]
        public int ReuseCount { get; set; }
    }
}
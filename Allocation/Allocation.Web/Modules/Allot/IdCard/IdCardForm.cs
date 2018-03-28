
namespace Allocation.Allot.Forms
{
    using Serenity;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.IO;

    [FormScript("Allot.IdCard")]
    [BasedOnRow(typeof(Entities.IdCardRow))]
    public class IdCardForm
    {
        public String AuthenticationType { get; set; }
        public String Name { get; set; }
        public String CardNo { get; set; }
        public String CheckResult { get; set; }
        public DateTime RequestDate { get; set; }
        public String RequestIp { get; set; }
        public Decimal Price { get; set; }
        [DefaultValue(0)]
        public Int32 ReusedCount { get; set; }
    }
}
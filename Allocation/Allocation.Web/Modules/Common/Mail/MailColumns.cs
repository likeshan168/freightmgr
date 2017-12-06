
namespace Allocation.Common.Columns
{
    using Serenity;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.IO;

    [ColumnsScript("Common.Mail")]
    [BasedOnRow(typeof(Entities.MailRow))]
    public class MailColumns
    {
        [EditLink, DisplayName("Db.Shared.RecordId"), AlignRight, SortOrder(-1)]
        public Int64 MailId { get; set; }
        [Hidden]
        public Guid Uid { get; set; }
        [EditLink, Width(300)]
        public String Subject { get; set; }
        [Width(180)]
        public String MailFrom { get; set; }
        [Width(180)]
        public String MailTo { get; set; }
        [Hidden]
        public String ReplyTo { get; set; }
        [Hidden]
        public String Cc { get; set; }
        [Hidden]
        public String Bcc { get; set; }
        [Width(80)]
        public Int32 Priority { get; set; }
        [Width(80), QuickFilter]
        public Int32 Status { get; set; }
        [Width(140)]
        public DateTime LockExpiration { get; set; }
        [Width(100)]
        public Int32 RetryCount { get; set; }
        [Width(140)]
        public DateTime SentDate { get; set; }
    }
}
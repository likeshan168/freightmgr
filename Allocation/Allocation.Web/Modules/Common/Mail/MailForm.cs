
namespace Allocation.Common.Forms
{
    using Serenity;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.IO;

    [FormScript("Common.Mail")]
    [BasedOnRow(typeof(Entities.MailRow))]
    public class MailForm
    {
        public Guid Uid { get; set; }
        public String Subject { get; set; }
        [HtmlNoteContentEditor, ReadOnly(true)]
        public String Body { get; set; }
        public String MailFrom { get; set; }
        public String MailTo { get; set; }
        public String ReplyTo { get; set; }
        public String Cc { get; set; }
        public String Bcc { get; set; }
        public Int32 Priority { get; set; }
        public Int32 Status { get; set; }
        public Int32 RetryCount { get; set; }
        public DateTime LockExpiration { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime SentDate { get; set; }
        public Int32 InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
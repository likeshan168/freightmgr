
namespace Allocation.Common.Entities
{
    using Serenity.ComponentModel;
    using Serenity.Data;
    using Serenity.Data.Mapping;
    using System;
    using System.ComponentModel;

    [ConnectionKey("Default"), TableName("[dbo].[Mail]"), DisplayName("Mail Queue"), InstanceName("Mail"), TwoLevelCached]
    [ReadPermission("Administration:General")]
    [ModifyPermission("Administration:General")]
    public sealed class MailRow : Row, IIdRow, INameRow, IInsertLogRow
    {
        [DisplayName("Mail Id"), Identity]
        public Int64? MailId
        {
            get { return Fields.MailId[this]; }
            set { Fields.MailId[this] = value; }
        }

        [DisplayName("Uid"), Column("UID"), NotNull, ReadOnly(true)]
        public Guid? Uid
        {
            get { return Fields.Uid[this]; }
            set { Fields.Uid[this] = value; }
        }

        [DisplayName("Subject"), Size(400), NotNull, QuickSearch, ReadOnly(true)]
        public String Subject
        {
            get { return Fields.Subject[this]; }
            set { Fields.Subject[this] = value; }
        }

        [DisplayName("Body"), ReadOnly(true)]
        public String Body
        {
            get { return Fields.Body[this]; }
            set { Fields.Body[this] = value; }
        }

        [DisplayName("Mail From"), Size(100), ReadOnly(true)]
        public String MailFrom
        {
            get { return Fields.MailFrom[this]; }
            set { Fields.MailFrom[this] = value; }
        }

        [DisplayName("Mail To"), Size(2000), ReadOnly(true)]
        public String MailTo
        {
            get { return Fields.MailTo[this]; }
            set { Fields.MailTo[this] = value; }
        }

        [DisplayName("Reply To"), Size(100), ReadOnly(true)]
        public String ReplyTo
        {
            get { return Fields.ReplyTo[this]; }
            set { Fields.ReplyTo[this] = value; }
        }

        [DisplayName("Cc"), Column("CC"), Size(2000), ReadOnly(true)]
        public String Cc
        {
            get { return Fields.Cc[this]; }
            set { Fields.Cc[this] = value; }
        }

        [DisplayName("Bcc"), Column("BCC"), Size(2000), ReadOnly(true)]
        public String Bcc
        {
            get { return Fields.Bcc[this]; }
            set { Fields.Bcc[this] = value; }
        }

        [DisplayName("Priority"), NotNull, ReadOnly(true)]
        public MailQueuePriority? Priority
        {
            get { return (MailQueuePriority?)Fields.Priority[this]; }
            set { Fields.Priority[this] = (Int32?)value; }
        }

        [DisplayName("Error Message"), ReadOnly(true)]
        public String ErrorMessage
        {
            get { return Fields.ErrorMessage[this]; }
            set { Fields.ErrorMessage[this] = value; }
        }

        [DisplayName("Status"), NotNull]
        public MailStatus? Status
        {
            get { return (MailStatus?)Fields.Status[this]; }
            set { Fields.Status[this] = (Int32?)value; }
        }

        [DisplayName("Retry Count"), NotNull]
        public Int32? RetryCount
        {
            get { return Fields.RetryCount[this]; }
            set { Fields.RetryCount[this] = value; }
        }

        [DisplayName("Lock Expiration"), NotNull, DateTimeEditor, DisplayFormat("s")]
        public DateTime? LockExpiration
        {
            get { return Fields.LockExpiration[this]; }
            set { Fields.LockExpiration[this] = value; }
        }

        [DisplayName("Sent Date"), DateTimeKind(DateTimeKind.Local), DateTimeEditor, ReadOnly(true)]
        public DateTime? SentDate
        {
            get { return Fields.SentDate[this]; }
            set { Fields.SentDate[this] = value; }
        }

        [DisplayName("Insert User"), ForeignKey(typeof(Administration.Entities.UserRow)), LeftJoin("insusr"), ReadOnly(true)]
        public Int32? InsertUserId
        {
            get { return Fields.InsertUserId[this]; }
            set { Fields.InsertUserId[this] = value; }
        }

        [DisplayName("Insert Date"), NotNull, DateTimeEditor, ReadOnly(true)]
        public DateTime? InsertDate
        {
            get { return Fields.InsertDate[this]; }
            set { Fields.InsertDate[this] = value; }
        }

        [Origin("insusr", "DisplayName"), DisplayName("Insert User"), ReadOnly(true)]
        public String InsertUser
        {
            get { return Fields.InsertUser[this]; }
            set { Fields.InsertUser[this] = value; }
        }

        IIdField IIdRow.IdField
        {
            get { return Fields.MailId; }
        }

        StringField INameRow.NameField
        {
            get { return Fields.Subject; }
        }

        public IIdField InsertUserIdField
        {
            get { return Fields.InsertUserId; }
        }

        public DateTimeField InsertDateField
        {
            get { return Fields.InsertDate; }
        }

        public static readonly RowFields Fields = new RowFields().Init();

        public MailRow()
            : base(Fields)
        {
        }

        public class RowFields : RowFieldsBase
        {
            public Int64Field MailId;
            public GuidField Uid;
            public StringField Subject;
            public StringField Body;
            public StringField MailFrom;
            public StringField MailTo;
            public StringField ReplyTo;
            public StringField Cc;
            public StringField Bcc;
            public Int32Field Priority;
            public Int32Field Status;
            public Int32Field RetryCount;
            public StringField ErrorMessage;
            public DateTimeField LockExpiration;
            public DateTimeField SentDate;
            public Int32Field InsertUserId;
            public DateTimeField InsertDate;
            public StringField InsertUser;

            public RowFields()
                : base()
            {
                LocalTextPrefix = "Common.Mail";
            }
        }
    }
}

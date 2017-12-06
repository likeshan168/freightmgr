
namespace Allocation.Common.Entities
{
    using Newtonsoft.Json;
    using Serenity.Data;
    using Serenity.Data.Mapping;
    using System;
    using System.ComponentModel;

    [ConnectionKey("Default"), TableName("BackgroundTaskLog"), DisplayName("BackgroundTasks"), InstanceName("BackgroundTasks"), TwoLevelCached]
    [ReadPermission("Administration:General")]
    [ModifyPermission("Administration:General")]
    [JsonConverter(typeof(JsonRowConverter))]
    public sealed class BackgroundTaskLogRow : Row, IIdRow, INameRow
    {
        [DisplayName("LogId"), Column("LogId"), Identity]
        public Int64? LogId
        {
            get { return Fields.LogId[this]; }
            set { Fields.LogId[this] = value; }
        }

        [DisplayName("Task Key"), Size(100), NotNull, QuickSearch]
        public String TaskKey
        {
            get { return Fields.TaskKey[this]; }
            set { Fields.TaskKey[this] = value; }
        }

        [DisplayName("Run At")]
        public DateTime? RunAt
        {
            get { return Fields.RunAt[this]; }
            set { Fields.RunAt[this] = value; }
        }

        [DisplayName("Status")]
        public Int32? Status
        {
            get { return Fields.Status[this]; }
            set { Fields.Status[this] = value; }
        }

        [DisplayName("Server"), Size(100)]
        public String Server
        {
            get { return Fields.Server[this]; }
            set { Fields.Server[this] = value; }
        }

        [DisplayName("Message")]
        public String Message
        {
            get { return Fields.Message[this]; }
            set { Fields.Message[this] = value; }
        }

        IIdField IIdRow.IdField
        {
            get { return Fields.LogId; }
        }

        StringField INameRow.NameField
        {
            get { return Fields.TaskKey; }
        }

        public static readonly RowFields Fields = new RowFields().Init();

        public BackgroundTaskLogRow()
            : base(Fields)
        {
        }

        public class RowFields : RowFieldsBase
        {
            public readonly Int64Field LogId;
            public readonly StringField TaskKey;
            public readonly DateTimeField RunAt;
            public readonly Int32Field Status;
            public readonly StringField Server;
            public readonly StringField Message;

            public RowFields()
            {
                LocalTextPrefix = "Common.BackgroundTask";
            }
        }
    }
}
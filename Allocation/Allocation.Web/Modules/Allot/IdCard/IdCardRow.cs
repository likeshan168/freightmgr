
namespace Allocation.Allot.Entities
{
    using Serenity;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using Serenity.Data.Mapping;
    using System;
    using System.ComponentModel;
    using System.IO;

    [ConnectionKey("Default"), TableName("[allot].[IdCard]")]
    [DisplayName("Id Card"), InstanceName("Id Card"), TwoLevelCached]
    [ReadPermission(AllotPermissionKeys.Api.View)]
    [ModifyPermission(AllotPermissionKeys.Api.Modify)]
    public sealed class IdCardRow : Row, IIdRow, INameRow
    {
        [DisplayName("Id"), Identity]
        public Int64? Id
        {
            get { return Fields.Id[this]; }
            set { Fields.Id[this] = value; }
        }

        [DisplayName("Authentication Type"), Size(255), NotNull, QuickSearch]
        public String AuthenticationType
        {
            get { return Fields.AuthenticationType[this]; }
            set { Fields.AuthenticationType[this] = value; }
        }

        [DisplayName("Name"), Size(30), NotNull, QuickSearch]
        public String Name
        {
            get { return Fields.Name[this]; }
            set { Fields.Name[this] = value; }
        }

        [DisplayName("Card No"), Size(50), NotNull, QuickSearch]
        public String CardNo
        {
            get { return Fields.CardNo[this]; }
            set { Fields.CardNo[this] = value; }
        }

        [DisplayName("Check Result"), Size(50), NotNull, QuickSearch]
        public String CheckResult
        {
            get { return Fields.CheckResult[this]; }
            set { Fields.CheckResult[this] = value; }
        }

        [DisplayName("Request Date"), NotNull]
        public DateTime? RequestDate
        {
            get { return Fields.RequestDate[this]; }
            set { Fields.RequestDate[this] = value; }
        }

        [DisplayName("Request Ip"), Size(30)]
        public String RequestIp
        {
            get { return Fields.RequestIp[this]; }
            set { Fields.RequestIp[this] = value; }
        }

        [DisplayName("Price"), Size(19), Scale(5)]
        public Decimal? Price
        {
            get { return Fields.Price[this]; }
            set { Fields.Price[this] = value; }
        }

        [DisplayName("ReusedCount"), NotNull]
        public Int32? ReusedCount
        {
            get { return Fields.ReusedCount[this]; }
            set { Fields.ReusedCount[this] = value; }
        }


        IIdField IIdRow.IdField
        {
            get { return Fields.Id; }
        }

        StringField INameRow.NameField
        {
            get { return Fields.AuthenticationType; }
        }

        public static readonly RowFields Fields = new RowFields().Init();

        public IdCardRow()
            : base(Fields)
        {
        }

        public class RowFields : RowFieldsBase
        {
            public Int64Field Id;
            public StringField AuthenticationType;
            public StringField Name;
            public StringField CardNo;
            public StringField CheckResult;
            public DateTimeField RequestDate;
            public StringField RequestIp;
            public DecimalField Price;
            public Int32Field ReusedCount;

            public RowFields()
                : base()
            {
                LocalTextPrefix = "Allot.IdCard";
            }
        }
    }
}

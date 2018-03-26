
namespace Allocation.Allot.Entities
{
    using Serenity;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using Serenity.Data.Mapping;
    using System;
    using System.ComponentModel;
    using System.IO;

    [ConnectionKey("Default"), TableName("[allot].[API]")]
    [DisplayName("Api"), InstanceName("Api"), TwoLevelCached]
    [ReadPermission(AllotPermissionKeys.Api.View)]
    [ModifyPermission(AllotPermissionKeys.Api.Modify)]
    [LookupScript("Allot.Api")]
    public sealed class ApiRow : Row, IIdRow, INameRow
    {
        [DisplayName("Id"), Identity]
        public Int64? Id
        {
            get { return Fields.Id[this]; }
            set { Fields.Id[this] = value; }
        }

        [DisplayName("Title"), Size(100), NotNull, QuickSearch]
        public String Title
        {
            get { return Fields.Title[this]; }
            set { Fields.Title[this] = value; }
        }

        [DisplayName("Api Url"), Size(50), NotNull]
        public String ApiUrl
        {
            get { return Fields.ApiUrl[this]; }
            set { Fields.ApiUrl[this] = value; }
        }

        [DisplayName("User Name"), Size(50), NotNull]
        public String UserName
        {
            get { return Fields.UserName[this]; }
            set { Fields.UserName[this] = value; }
        }

        [DisplayName("Password"), Size(50), NotNull]
        public String Password
        {
            get { return Fields.Password[this]; }
            set { Fields.Password[this] = value; }
        }

        [DisplayName("App Key"), Size(400), NotNull]
        public String AppKey
        {
            get { return Fields.AppKey[this]; }
            set { Fields.AppKey[this] = value; }
        }

        [DisplayName("Price"), Size(19), Scale(5), NotNull]
        public Decimal? Price
        {
            get { return Fields.Price[this]; }
            set { Fields.Price[this] = value; }
        }

        [DisplayName("Is Enabled"), NotNull]
        public Boolean? IsEnabled
        {
            get { return Fields.IsEnabled[this]; }
            set { Fields.IsEnabled[this] = value; }
        }

        [DisplayName("ReuseCount"), NotNull]
        public Int32? ReuseCount
        {
            get { return Fields.ReuseCount[this]; }
            set { Fields.ReuseCount[this] = value; }
        }

        IIdField IIdRow.IdField
        {
            get { return Fields.Id; }
        }

        StringField INameRow.NameField
        {
            get { return Fields.Title; }
        }

        public static readonly RowFields Fields = new RowFields().Init();

        public ApiRow()
            : base(Fields)
        {
        }

        public class RowFields : RowFieldsBase
        {
            public Int64Field Id;
            public StringField Title;
            public StringField ApiUrl;
            public StringField UserName;
            public StringField Password;
            public StringField AppKey;
            public DecimalField Price;
            public BooleanField IsEnabled;
            public Int32Field ReuseCount;

            public RowFields()
                : base()
            {
                LocalTextPrefix = "Allot.Api";
            }
        }
    }
}

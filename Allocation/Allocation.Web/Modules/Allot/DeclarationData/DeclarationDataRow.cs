
namespace Allocation.Allot.Entities
{
    using Serenity;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using Serenity.Data.Mapping;
    using System;
    using System.ComponentModel;
    using System.IO;

    [ConnectionKey("Default"), TableName("[allot].[DeclarationData]")]
    [DisplayName("Declaration Data"), InstanceName("Declaration Data"), TwoLevelCached]
    [ReadPermission(PermissionKeys.DeclarationData.View)]
    [ModifyPermission(PermissionKeys.DeclarationData.Modify)]
    public sealed class DeclarationDataRow : Row, IIdRow, INameRow
    {
        [DisplayName("Id"), Identity]
        public Int64? Id
        {
            get { return Fields.Id[this]; }
            set { Fields.Id[this] = value; }
        }

        [DisplayName("Application Unit"), Size(200), QuickSearch]
        public String ApplicationUnit
        {
            get { return Fields.ApplicationUnit[this]; }
            set { Fields.ApplicationUnit[this] = value; }
        }

        [DisplayName("Master Awb"), Size(50), NotNull, QuickSearch]
        public String MasterAwb
        {
            get { return Fields.MasterAwb[this]; }
            set { Fields.MasterAwb[this] = value; }
        }

        [DisplayName("Sub Awb"), Size(50), NotNull, QuickSearch]
        public String SubAwb
        {
            get { return Fields.SubAwb[this]; }
            set { Fields.SubAwb[this] = value; }
        }

        [DisplayName("Amount"), NotNull]
        public Int32? Amount
        {
            get { return Fields.Amount[this]; }
            set { Fields.Amount[this] = value; }
        }

        [DisplayName("Weight")]
        public Double? Weight
        {
            get { return Fields.Weight[this]; }
            set { Fields.Weight[this] = value; }
        }

        [DisplayName("Description"), Size(200)]
        public String Description
        {
            get { return Fields.Description[this]; }
            set { Fields.Description[this] = value; }
        }

        //[DisplayName("Is Checked")]
        //public Boolean? IsChecked
        //{
        //    get { return Fields.IsChecked[this]; }
        //    set { Fields.IsChecked[this] = value; }
        //}

        [DisplayName("IsChecked"), NotNull, DefaultValue(StateKind.NoChecked)]
        public StateKind? IsChecked
        {
            get { return (StateKind?)Fields.IsChecked[this]; }
            set { Fields.IsChecked[this] = (Int32?)value; }
        }

        [DisplayName("Flight"), Size(100), QuickSearch]
        public String Flight
        {
            get { return Fields.Flight[this]; }
            set { Fields.Flight[this] = value; }
        }

        IIdField IIdRow.IdField
        {
            get { return Fields.Id; }
        }

        StringField INameRow.NameField
        {
            get { return Fields.ApplicationUnit; }
        }

        public static readonly RowFields Fields = new RowFields().Init();

        public DeclarationDataRow()
            : base(Fields)
        {
        }

        public class RowFields : RowFieldsBase
        {
            public Int64Field Id;
            public StringField ApplicationUnit;
            public StringField MasterAwb;
            public StringField SubAwb;
            public Int32Field Amount;
            public DoubleField Weight;
            public StringField Description;
            public StringField Flight;
            public Int32Field IsChecked;

            public RowFields()
                : base()
            {
                LocalTextPrefix = "Allot.DeclarationData";
            }
        }
    }
}

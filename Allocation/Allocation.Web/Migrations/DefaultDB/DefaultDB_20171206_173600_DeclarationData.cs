using FluentMigrator;

namespace Allocation.Migrations.DefaultDB
{
    [Migration(20171206173600)]
    public class DefaultDB_20171206_173600_DeclarationData : AutoReversingMigration
    {
        public override void Up()
        {
            if (!Schema.Schema("allot").Exists())
            {
                Create.Schema("allot");
            }
            this.CreateTableWithId64("DeclarationData", "Id", s => s
                .WithColumn("ApplicationUnit").AsString(200).Nullable()
                .WithColumn("MasterAwb").AsString(50).NotNullable()
                .WithColumn("SubAwb").AsString(50).NotNullable()
                .WithColumn("Amount").AsInt32().NotNullable()
                .WithColumn("Weight").AsDouble().Nullable()
                .WithColumn("Description").AsString(200).Nullable()
                .WithColumn("IsChecked").AsBoolean().Nullable()
                , "allot", true);
        }
    }
}
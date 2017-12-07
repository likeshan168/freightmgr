using FluentMigrator;

namespace Allocation.Migrations.DefaultDB
{
    [Migration(20171207152700)]
    public class DefaultDB_20171207_152700_AddFlightInDeclarationData : AutoReversingMigration
    {
        public override void Up()
        {
            if (!Schema.Schema("allot").Exists())
            {
                Create.Schema("allot");
            }
            Alter.Table("DeclarationData").InSchema("allot")
                .AddColumn("Flight").AsString(100).Nullable();
        }
    }
}
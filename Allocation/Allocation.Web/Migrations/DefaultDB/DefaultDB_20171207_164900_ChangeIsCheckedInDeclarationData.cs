using FluentMigrator;

namespace Allocation.Migrations.DefaultDB
{
    [Migration(20171207164900)]
    public class DefaultDB_20171207_164900_ChangeIsCheckedInDeclarationData : AutoReversingMigration
    {
        public override void Up()
        {
            if (!Schema.Schema("allot").Exists())
            {
                Create.Schema("allot");
            }
            Alter.Table("DeclarationData").InSchema("allot")
                .AlterColumn("IsChecked").AsInt32().Nullable();
        }
    }
}
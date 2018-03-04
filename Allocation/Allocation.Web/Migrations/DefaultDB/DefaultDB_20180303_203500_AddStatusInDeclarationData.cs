using FluentMigrator;

namespace Allocation.Migrations.DefaultDB
{
    [Migration(20180303203500)]
    public class DefaultDB_20180303_203500_AddStatusInDeclarationData : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("DeclarationData").InSchema("allot")
                  .AddColumn("Status").AsString(20).Nullable();
        }
    }
}
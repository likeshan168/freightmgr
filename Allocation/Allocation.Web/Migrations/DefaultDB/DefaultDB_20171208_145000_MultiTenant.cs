using FluentMigrator;

namespace Allocation.Migrations.DefaultDB
{
    [Migration(20171208145000)]
    public class DefaultDB_20171208_145000_MultiTenant : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("DeclarationData").InSchema("allot")
                .AddColumn("TenantId").AsInt32()
                .NotNullable().WithDefaultValue(1);
        }
    }
}
using FluentMigrator;

namespace Allocation.Migrations.DefaultDB
{
    [Migration(20171208144900)]
    public class DefaultDB_20171208_144900_MultiTenant : AutoReversingMigration
    {
        public override void Up()
        {
            this.CreateTableWithId32("Tenants", "TenantId", s => s
                .WithColumn("TenantName").AsString(100)
                .NotNullable());

            Insert.IntoTable("Tenants")
                .Row(new
                {
                    TenantName = "Primary Tenant"
                });

            Insert.IntoTable("Tenants")
                .Row(new
                {
                    TenantName = "Second Tenant"
                });

            Insert.IntoTable("Tenants")
                .Row(new
                {
                    TenantName = "Third Tenant"
                });

            Alter.Table("Users")
                .AddColumn("TenantId").AsInt32()
                .NotNullable().WithDefaultValue(1);

            Alter.Table("Roles")
                .AddColumn("TenantId").AsInt32()
                .NotNullable().WithDefaultValue(1);

            Alter.Table("Languages")
                .AddColumn("TenantId").AsInt32()
                .NotNullable().WithDefaultValue(1);
        }
    }
}
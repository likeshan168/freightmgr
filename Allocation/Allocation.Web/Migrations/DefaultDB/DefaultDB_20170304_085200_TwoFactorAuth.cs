using FluentMigrator;

namespace Allocation.Migrations.DefaultDB
{
    [Migration(20170304085200)]
    public class DefaultDB_20170304_085200_TwoFactorAuth : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Users")
                .AddColumn("MobilePhoneNumber").AsString(20).Nullable()
                .AddColumn("MobilePhoneVerified").AsBoolean().NotNullable().WithDefaultValue(false)
                .AddColumn("TwoFactorAuth").AsInt32().Nullable();
        }
    }
}
using FluentMigrator;

namespace Allocation.Migrations.DefaultDB
{
    [Migration(20180326101900)]
    public class DefaultDB_20180326_101900_IdCard : AutoReversingMigration
    {
        public override void Up()
        {
            if (!Schema.Schema("allot").Exists())
            {
                Create.Schema("allot");
            }
            this.CreateTableWithId64("IdCard", "Id", s => s
                .WithColumn("AuthenticationType").AsString().NotNullable()
                .WithColumn("Name").AsString(30).NotNullable()
                .WithColumn("CardNo").AsString(50).NotNullable()
                .WithColumn("CheckResult").AsString(50).NotNullable()
                .WithColumn("RequestDate").AsDateTime().NotNullable()
                .WithColumn("RequestIp").AsString(30).Nullable()
                .WithColumn("Price").AsDecimal().Nullable()
                , "allot", true);
        }
    }
}
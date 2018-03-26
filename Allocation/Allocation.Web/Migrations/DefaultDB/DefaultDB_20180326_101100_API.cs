using FluentMigrator;

namespace Allocation.Migrations.DefaultDB
{
    [Migration(20180326101100)]
    public class DefaultDB_20180326_101100_API : AutoReversingMigration
    {
        public override void Up()
        {
            if (!Schema.Schema("allot").Exists())
            {
                Create.Schema("allot");
            }
            this.CreateTableWithId64("API", "Id", s => s
                .WithColumn("Title").AsString(100).NotNullable()
                .WithColumn("ApiUrl").AsString(50).NotNullable()
                .WithColumn("UserName").AsString(50).NotNullable()
                .WithColumn("Password").AsString(50).NotNullable()
                .WithColumn("AppKey").AsString(400).NotNullable()
                .WithColumn("Price").AsDecimal().NotNullable().WithColumnDescription("就是多少钱一条")
                .WithColumn("IsEnabled").AsBoolean().NotNullable()
                , "allot", true);
        }
    }
}
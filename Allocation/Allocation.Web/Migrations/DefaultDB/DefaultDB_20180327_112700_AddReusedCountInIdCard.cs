using FluentMigrator;

namespace Allocation.Migrations.DefaultDB
{
    [Migration(20180327112700)]
    public class DefaultDB_20180327_112700_AddReusedCountInIdCard : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("IdCard").InSchema("allot")
                  .AddColumn("ReusedCount").AsInt32().Nullable().WithColumnDescription("已经重复利用的次数");
        }
    }
}
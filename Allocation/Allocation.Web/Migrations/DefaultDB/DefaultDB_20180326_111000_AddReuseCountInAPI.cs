using FluentMigrator;

namespace Allocation.Migrations.DefaultDB
{
    [Migration(20180326111000)]
    public class DefaultDB_20180326_111000_AddReuseCountInAPI : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("API").InSchema("allot")
                  .AddColumn("ReuseCount").AsInt32().NotNullable();
        }
    }
}
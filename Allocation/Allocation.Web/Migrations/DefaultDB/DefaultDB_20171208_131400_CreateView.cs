using FluentMigrator;

namespace Allocation.Migrations.DefaultDB
{
    [Migration(20171208131400)]
    public class DefaultDB_20171208_131400_CreateView : AutoReversingMigration
    {
        public override void Up()
        {
            //Execute.Sql("if exists(select * from sys.objects where type ='V' and name='AbnormalResultsView') Drop view [allot].[AbnormalResultsView]");
            //Execute.EmbeddedScript("AbnormalResultsView.sql");

            //Execute.Sql("if exists(select * from sys.objects where type ='V' and name='NormalResultsView') Drop view [allot].[NormalResultsView]");
            //Execute.EmbeddedScript("NormalResultsView.sql");
        }
    }
}
using FluentMigrator;

namespace Persistence.Migrator.Migrations
{
    [Migration(1)]
    internal class Migration1 : BaseMigration
    {
        public override void Up()
        {
            ExecuteSqlFile("Migration1.sql");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}

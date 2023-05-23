using FluentMigrator;
using System.Reflection;

namespace Persistence.Migrator.Migrations
{
    internal abstract class BaseMigration : Migration
    {
        protected void ExecuteSqlFile(string fileName)
        {
            var path = Path.Combine(Assembly.GetExecutingAssembly().Location, "Migrations", "SqlFiles", fileName);
            var sql = File.ReadAllText(path);


        }
    }
}

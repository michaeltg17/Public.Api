using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Dac;
using Testcontainers.MsSql;

namespace IntegrationTests.Others
{
    internal static class Database
    {
        public static string ConnectionString { get; private set; } = default!;

        const string DatabaseName = "Database";
        static MsSqlContainer sqlServerContainer = default!;

        static async Task Database()
        {
            CreateContainer();
            DeployDacpac();
        }

        static async Task CreateContainer()
        {
            sqlServerContainer = new MsSqlBuilder()
                .WithName("IntegrationTestsSqlServer")
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .Build();

            var builder = new SqlConnectionStringBuilder(sqlServerContainer.GetConnectionString())
            {
                InitialCatalog = DatabaseName
            };

            ConnectionString = builder.ConnectionString;

            await sqlServerContainer.StartAsync();
        }

        static void DeployDacpac()
        {
            var services = new DacServices(ConnectionString);
            var package = DacPackage.Load("Public.Database.dacpac");
            services.Deploy(package, DatabaseName);
        }
    }
}

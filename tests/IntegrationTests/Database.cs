using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Dac;
using Testcontainers.MsSql;

namespace IntegrationTests
{
    public class Database : IAsyncDisposable
    {
        const bool KeepAlive = true;

        public string ConnectionString { get; private set; } = default!;

        const string DatabaseName = "Database";
        const string ContainerName = "IntegrationTestsSqlServer";
        const int HostPort = 50000;
        MsSqlContainer sqlServerContainer = default!;

        Database() { }

        public static async Task<Database> Create()
        {
            var database = new Database();
            var existsContainer = await database.UseExistingContainerIfExists();
            if (!existsContainer) await database.CreateContainer();
            database.DeployDacpac();
            return database;
        }

        async Task<bool> UseExistingContainerIfExists()
        {
            var client = new DockerClientConfiguration().CreateClient();
            var parameters = new ContainersListParameters() { All = true };
            var containers = await client.Containers.ListContainersAsync(parameters);
            var container = containers.SingleOrDefault(c => c.Names.Contains("/" + ContainerName));
            if (container == null) return false;

            var builder = new SqlConnectionStringBuilder()
            {
                InitialCatalog = DatabaseName,
                UserID = MsSqlBuilder.DefaultUsername,
                Password = MsSqlBuilder.DefaultPassword,
                DataSource = "localhost," + HostPort,
                TrustServerCertificate = true
            };

            ConnectionString = builder.ConnectionString;

            return true;
        }

        async Task CreateContainer()
        {
            sqlServerContainer = new MsSqlBuilder()
                .WithName(ContainerName)
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPortBinding(HostPort, 1433)
                .WithCleanUp(!KeepAlive)
                .WithAutoRemove(!KeepAlive)
                .Build();

            await sqlServerContainer.StartAsync();

            var builder = new SqlConnectionStringBuilder(sqlServerContainer.GetConnectionString())
            {
                InitialCatalog = DatabaseName
            };

            ConnectionString = builder.ConnectionString;

            await sqlServerContainer.StartAsync();
        }

        void DeployDacpac()
        {
            var services = new DacServices(ConnectionString);
            var package = DacPackage.Load(@"Public.Database.dacpac");
            services.Deploy(package, DatabaseName, true);
        }

        public ValueTask DisposeAsync()
        {
            if (KeepAlive)
            {
                return ValueTask.CompletedTask;
            }

#pragma warning disable CS0162 // Unreachable code detected
            GC.SuppressFinalize(this);
            return sqlServerContainer.DisposeAsync();
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
}

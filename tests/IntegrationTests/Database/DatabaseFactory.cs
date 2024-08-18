using Core;
using Xunit.Abstractions;
using IntegrationTests.Settings;
using Docker.DotNet.Models;
using Docker.DotNet;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Dac;
using Testcontainers.MsSql;
using Xunit.Sdk;

namespace IntegrationTests.Database
{
    internal class DatabaseFactory(IMessageSink messageSink, ITestSettings testSettings) : IFactory<Database>
    {
        const string DatabaseName = "Database";
        const string ContainerName = "IntegrationTestsSqlServer";
        const int HostPort = 50000;

        public async Task<Database> Create()
        {
            WriteMessage("Initializing database.");

            WriteMessage("Using existing container if exists.");
            string connectionString;
            MsSqlContainer? container = default;
            if (await ExistsContainer())
            {
                connectionString = GetConnectionString();
            }
            else
            {
                WriteMessage("Does not exist. Creating new container.");
                container = await CreateContainer();
                WriteMessage("Container created.");
                connectionString = GetConnectionString(container);
            }

            if (testSettings.ShouldDeployDacpac)
            {
                WriteMessage("Deploying dacpac.");
                DeployDacpac(connectionString);
            }

            WriteMessage("Database initialized.");
            return new Database(testSettings, container) { ConnectionString = connectionString };
        }

        static async Task<bool> ExistsContainer()
        {
            var client = new DockerClientConfiguration().CreateClient();
            var parameters = new ContainersListParameters() { All = true };
            var containers = await client.Containers.ListContainersAsync(parameters);
            var container = containers.SingleOrDefault(c => c.Names.Contains("/" + ContainerName));
            return container != null;
        }

        async Task<MsSqlContainer> CreateContainer()
        {
            var sqlServerContainer = new MsSqlBuilder()
                .WithName(ContainerName)
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPortBinding(HostPort, 1433)
                .WithCleanUp(!testSettings.KeepAliveDatabase)
                .WithAutoRemove(!testSettings.KeepAliveDatabase)
                .Build();

            await sqlServerContainer.StartAsync();

            return sqlServerContainer;
        }

        static string GetConnectionString(MsSqlContainer container)
        {
            var builder = new SqlConnectionStringBuilder(container.GetConnectionString())
            {
                InitialCatalog = DatabaseName
            };

            return builder.ConnectionString;
        }

        static string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder()
            {
                InitialCatalog = DatabaseName,
                UserID = MsSqlBuilder.DefaultUsername,
                Password = MsSqlBuilder.DefaultPassword,
                DataSource = "localhost," + HostPort,
                TrustServerCertificate = true
            };

            return builder.ConnectionString;
        }

        void DeployDacpac(string connectionString)
        {
            var services = new DacServices(connectionString);
            services.Message += (sender, e) => WriteMessage(e.Message.ToString());
            var package = DacPackage.Load(@"Public.Database.dacpac");
            services.Deploy(package, DatabaseName, true);
        }

        void WriteMessage(string message) => messageSink.OnMessage(new DiagnosticMessage(message));
    }
}

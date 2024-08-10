using Docker.DotNet;
using Docker.DotNet.Models;
using IntegrationTests.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Dac;
using Testcontainers.MsSql;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace IntegrationTests
{
    public class Database : IAsyncDisposable
    {
        const string DatabaseName = "Database";
        const string ContainerName = "IntegrationTestsSqlServer";
        const int HostPort = 50000;

        public string ConnectionString { get; private set; } = default!;

        MsSqlContainer? sqlServerContainer;
        readonly IMessageSink messageSink;
        readonly ITestSettings testSettings;

        Database(IMessageSink messageSink, ITestSettings testSettings)
        {
            this.messageSink = messageSink;
            this.testSettings = testSettings;
        }

        public static async Task<Database> Initialize(IMessageSink messageSink, ITestSettings testSettings)
        {
            var database = new Database(messageSink, testSettings);
            database.WriteMessage("Initializing database.");

            database.WriteMessage("Using existing container if exists.");
            var existsContainer = await database.UseExistingContainerIfExists();

            if (!existsContainer)
            {
                database.WriteMessage("Does not exist. Creating new container.");
                await database.CreateContainer();
                database.WriteMessage("Container created.");
            }
            
            if (testSettings.ShouldDeployDacpac)
            {
                database.WriteMessage("Deploying dacpac.");
                database.DeployDacpac();
            }

            database.WriteMessage("Database initialized.");
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
                .WithCleanUp(!testSettings.KeepAliveDatabase)
                .WithAutoRemove(!testSettings.KeepAliveDatabase)
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
            services.Message += (sender, e) => WriteMessage(e.Message.ToString());
            var package = DacPackage.Load(@"Public.Database.dacpac");
            services.Deploy(package, DatabaseName, true);
        }

        void WriteMessage(string message) => messageSink.OnMessage(new DiagnosticMessage(message));

        public ValueTask DisposeAsync()
        {
            if (testSettings.KeepAliveDatabase)
            {
                return ValueTask.CompletedTask;
            }

            GC.SuppressFinalize(this);
            return sqlServerContainer?.DisposeAsync() ?? ValueTask.CompletedTask;
        }
    }
}

using Docker.DotNet;
using Docker.DotNet.Models;
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
        MsSqlContainer? SqlServerContainer;
        readonly IMessageSink MessageSink;

        Database(IMessageSink messageSink)
        {
            MessageSink = messageSink;
        }

        public static async Task<Database> Initialize(IMessageSink messageSink)
        {
            var database = new Database(messageSink);
            database.WriteMessage("Initializing database.");

            database.WriteMessage("Using existing container if exists.");
            var existsContainer = await database.UseExistingContainerIfExists();

            if (!existsContainer)
            {
                database.WriteMessage("Does not exist. Creating new container.");
                await database.CreateContainer();
                database.WriteMessage("Container created.");
            }
            
            if (TestOptions.ShouldDeployDacpac)
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
            SqlServerContainer = new MsSqlBuilder()
                .WithName(ContainerName)
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPortBinding(HostPort, 1433)
                .WithCleanUp(!TestOptions.KeepAliveDatabase)
                .WithAutoRemove(!TestOptions.KeepAliveDatabase)
                .Build();

            await SqlServerContainer.StartAsync();

            var builder = new SqlConnectionStringBuilder(SqlServerContainer.GetConnectionString())
            {
                InitialCatalog = DatabaseName
            };

            ConnectionString = builder.ConnectionString;

            await SqlServerContainer.StartAsync();
        }

        void DeployDacpac()
        {
            var services = new DacServices(ConnectionString);
            services.Message += (sender, e) => WriteMessage(e.Message.ToString());
            var package = DacPackage.Load(@"Public.Database.dacpac");
            services.Deploy(package, DatabaseName, true);
        }

        void WriteMessage(string message) => MessageSink.OnMessage(new DiagnosticMessage(message));

        public ValueTask DisposeAsync()
        {
            if (TestOptions.KeepAliveDatabase)
            {
#pragma warning disable CS0162 // Unreachable code detected
                return ValueTask.CompletedTask;
#pragma warning restore CS0162 // Unreachable code detected
            }

            GC.SuppressFinalize(this);
            return SqlServerContainer?.DisposeAsync() ?? ValueTask.CompletedTask;
        }
    }
}

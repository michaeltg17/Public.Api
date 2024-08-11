using IntegrationTests.Settings;
using Testcontainers.MsSql;

namespace IntegrationTests.Database
{
    public class Database(ITestSettings testSettings, MsSqlContainer? sqlServerContainer) : IAsyncDisposable
    {
        public required string ConnectionString { get; init; }

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

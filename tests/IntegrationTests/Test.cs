using Xunit.Abstractions;
using Serilog.Sinks.InMemory;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests
{
    public abstract class Test : IAsyncLifetime
    {
        public ApiClient.ApiClient ApiClient { get; private set; } = default!;
        internal WebApplicationFactoryFixture WebApplicationFactoryFixture { get; set; } = default!;
        public ITestOutputHelper TestOutputHelper { get; set; } = default!;
        AppDbContext DbContext { get; set; } = default!;
        AsyncServiceScope Scope { get; set; } = default!;

        public void Initialize()
        {
            WebApplicationFactoryFixture.TestOutputHelper = TestOutputHelper;
            WebApplicationFactoryFixture.InMemorySink = new InMemorySink();
            ApiClient = new(WebApplicationFactoryFixture.CreateClient());

            Scope = WebApplicationFactoryFixture.Services.CreateAsyncScope();
            DbContext = Scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }

        Task<int> DeleteEntitiesFromDb()
        {
            var sql = 
                "DELETE FROM Images;" +
                "DELETE FROM ImageGroups;";
            return DbContext.Database.ExecuteSqlRawAsync(sql);
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await DeleteEntitiesFromDb();
            await Scope.DisposeAsync();
            WebApplicationFactoryFixture.FlushLogger();
        }

        //Forced implementation by IAsyncLifetime, to be removed once xunit v3 gets production ready
        public Task InitializeAsync() => Task.CompletedTask;
    }
}

using Xunit.Abstractions;
using Serilog.Sinks.InMemory;

namespace IntegrationTests
{
    public abstract class Test : IDisposable
    {
        public ApiClient.ApiClient ApiClient { get; private set; } = default!;
        internal WebApplicationFactoryFixture WebApplicationFactoryFixture { get; set; } = default!;
        public ITestOutputHelper TestOutputHelper { get; set; } = default!;

        public void Initialize()
        {
            WebApplicationFactoryFixture.TestOutputHelper = TestOutputHelper;
            WebApplicationFactoryFixture.InMemorySink = new InMemorySink();
            ApiClient = new(WebApplicationFactoryFixture.CreateClient());
        }

        public void Dispose()
        {
            WebApplicationFactoryFixture.FlushLogger();
            GC.SuppressFinalize(this);
        }
    }
}

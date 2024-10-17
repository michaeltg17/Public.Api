using Xunit.Abstractions;
using Serilog.Sinks.InMemory;
using Core.Testing.Helpers;

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

        public string GetTestFilePath(string fileName) => TestFileHelper.GetTestFilePath(GetType(), fileName);

        public void Dispose()
        {
            WebApplicationFactoryFixture.FlushLogger();
            GC.SuppressFinalize(this);
        }
    }
}

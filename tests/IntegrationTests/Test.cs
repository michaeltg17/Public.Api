using Xunit.Abstractions;

namespace IntegrationTests
{
    public abstract class Test : IDisposable
    {
        public ApiClient.ApiClient ApiClient { get; private set; } = default!;
        public WebApplicationFactoryFixture WebApplicationFactoryFixture { get; set; } = default!;
        public ITestOutputHelper TestOutputHelper { get; set; } = default!;

        public void Initialize()
        {
            WebApplicationFactoryFixture.TestOutputHelper = TestOutputHelper;
            ApiClient = new(WebApplicationFactoryFixture.CreateClient());
            WebApplicationFactoryFixture.FlushLogger();
            GC.SuppressFinalize(this);
        }
    }
}

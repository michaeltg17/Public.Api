using Xunit.Abstractions;

namespace IntegrationTests
{
    public abstract class Test : IDisposable
    {
        protected WebApplicationFactoryFixture WebApplicationFactoryFixture;
        protected readonly ApiClient.ApiClient apiClient;

        public Test(WebApplicationFactoryFixture webApplicationFactoryFixture, ITestOutputHelper testOutputHelper)
        {
            WebApplicationFactoryFixture = webApplicationFactoryFixture;
            webApplicationFactoryFixture.TestOutputHelper = testOutputHelper;
            apiClient = new(webApplicationFactoryFixture.CreateClient());
        }

        public void Dispose()
        {
            WebApplicationFactoryFixture.FlushLogger();
            GC.SuppressFinalize(this);
        }
    }
}

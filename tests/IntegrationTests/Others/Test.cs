using Client;
using Xunit.Abstractions;

namespace IntegrationTests.Others
{
    public abstract class Test
    {
        protected readonly ApiClient apiClient;

        public Test(ITestOutputHelper testOutputHelper, WebApplicationFactoryFixture factory)
        {
            factory.TestOutputHelper = testOutputHelper;
            apiClient = new(factory.CreateClient());
        }
    }
}

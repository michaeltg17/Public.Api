using Xunit.Abstractions;

namespace IntegrationTests
{
    public abstract class Test
    {
        protected readonly ApiClient.ApiClient apiClient;

        public Test(WebApplicationFactoryFixture factory, ITestOutputHelper testOutputHelper)
        {
            factory.TestOutputHelper = testOutputHelper;
            //factory.ReloadSerilog();
            apiClient = new(factory.CreateClient());
            testOutputHelper.WriteLine("Im test");

        }
    }
}

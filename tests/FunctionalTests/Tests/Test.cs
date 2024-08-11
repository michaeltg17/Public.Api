using FunctionalTests.Settings;

namespace FunctionalTests.Tests
{
    public abstract class Test
    {
        protected readonly ApiClient.ApiClient apiClient;

        public Test(ITestSettings settings)
        {
            apiClient = new ApiClient.ApiClient(new HttpClient() { BaseAddress = new Uri(settings.ApiUrl) });
        }
    }
}

using FunctionalTests.Settings;

namespace FunctionalTests.Tests
{
    public abstract class Test
    {
        internal ApiClient.ApiClient ApiClient { get; private set; } = default!;
        internal ITestSettings TestSettings { get; set; } = default!;

        internal void Initialize()
        {
            ApiClient = new ApiClient.ApiClient(new HttpClient() { BaseAddress = new Uri(TestSettings.ApiUrl) });
        }
    }
}

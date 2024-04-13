namespace FunctionalTests
{
    public abstract class Test
    {
        protected readonly ApiClient.ApiClient apiClient;

        public Test(ISettings settings)
        {
            apiClient = new ApiClient.ApiClient(new HttpClient() { BaseAddress = new Uri(settings.ApiUrl) });
        }
    }
}

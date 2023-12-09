using Client;

namespace FunctionalTests
{
    public abstract class Test
    {
        protected readonly ApiClient apiClient;

        public Test(ISettings settings)
        {
            apiClient = new ApiClient(new HttpClient() { BaseAddress = new Uri(settings.ApiUrl) });
        }
    }
}

using Client;

namespace FunctionalTests.Others
{
    public abstract class Test
    {
        protected readonly ApiClient apiClient;

        public Test() 
        {
            apiClient = new ApiClient(new HttpClient() { BaseAddress = new Uri("https://localhost:5001") });
        }
    }
}

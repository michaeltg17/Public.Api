using Client;

namespace FunctionalTests.Others
{
    public abstract class ApiTest
    {
        public ApiClient Client { get; set; }

        public ApiTest() 
        {
            Client = new ApiClient(new HttpClient() { BaseAddress = new Uri("https://localhost:5001") });
        }
    }
}

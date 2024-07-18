using ApiClient.Endpoints;

namespace ApiClient
{
    public class ApiClient(HttpClient httpClient)
    {
        public HttpClient HttpClient { get; } = httpClient;
        public ApiEndpoints Api { get; } = new(httpClient);
        public TestEndpoints TestController { get; } = new(httpClient, "TestController");
        public TestEndpoints TestMinimalApi { get; } = new(httpClient, "TestMinimalApi");

        public Task<HttpResponseMessage> RequestUnexistingRoute()
        {
            return HttpClient.GetAsync($"UnexistingRoute/UnexistingRoute");
        }
    }
}

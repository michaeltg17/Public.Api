using ApiClient.Endpoints;

namespace ApiClient
{
    public class ApiClient(HttpClient httpClient)
    {
        public HttpClient HttpClient { get; } = httpClient;
        public ApiEndpoints Api { get; } = new(httpClient);
        public TestEndpoints TestController { get; } = new(httpClient, nameof(TestController));
        public TestEndpoints TestMinimalApi { get; } = new(httpClient, nameof(TestMinimalApi));

        public TestEndpoints GetTestEndpoints(string name)
        {
            return name switch
            {
                nameof(TestController) => TestController,
                nameof(TestMinimalApi) => TestMinimalApi,
                _ => throw new ArgumentException($"Name '{name}' is not valid.")
            };
        }

        public Task<HttpResponseMessage> RequestUnexistingRoute()
        {
            return HttpClient.GetAsync($"UnexistingRoute/UnexistingRoute");
        }
    }
}

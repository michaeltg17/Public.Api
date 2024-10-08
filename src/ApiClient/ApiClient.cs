﻿using ApiClient.Endpoints;

namespace ApiClient
{
    public class ApiClient(HttpClient httpClient)
    {
        public HttpClient HttpClient { get; } = httpClient;
        public ApiEndpoints ControllerApi { get; } = new(httpClient, nameof(ControllerApi));
        public ApiEndpoints MinimalApi { get; } = new(httpClient, nameof(MinimalApi));
        public TestEndpoints TestControllerApi { get; } = new(httpClient, nameof(TestControllerApi));
        public TestEndpoints TestMinimalApi { get; } = new(httpClient, nameof(TestMinimalApi));

        public ApiEndpoints GetApiEndpoints(string apiType)
        {
            return apiType switch
            {
                nameof(ControllerApi) => ControllerApi,
                nameof(MinimalApi) => MinimalApi,
                _ => throw new ArgumentException($"Api type '{apiType}' is not valid.")
            };
        }

        public TestEndpoints GetTestEndpoints(string apiType)
        {
            return apiType switch
            {
                nameof(TestControllerApi) => TestControllerApi,
                nameof(TestMinimalApi) => TestMinimalApi,
                _ => throw new ArgumentException($"Api type '{apiType}' is not valid.")
            };
        }

        public Task<HttpResponseMessage> RequestUnexistingRoute()
        {
            return HttpClient.GetAsync($"UnexistingRoute/UnexistingRoute");
        }
    }
}

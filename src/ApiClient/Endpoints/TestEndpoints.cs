using Api.Models.Requests;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace ApiClient.Endpoints
{
    public class TestEndpoints(HttpClient httpClient, string baseRoute)
    {
        public Task<HttpResponseMessage> DeleteAllTestEntities()
        {
            return httpClient.DeleteAsync($"{baseRoute}/DeleteAllTestEntities");
        }

        public Task<HttpResponseMessage> ThrowInternalServerError()
        {
            return httpClient.PostAsync($"{baseRoute}/ThrowInternalServerError", null);
        }

        public Task<HttpResponseMessage> GetOk()
        {
            return httpClient.GetAsync($"{baseRoute}/GetOk");
        }

        public Task<HttpResponseMessage> Get(long id)
        {
            return Get((object)id);
        }

        public Task<HttpResponseMessage> Get(object id)
        {
            return httpClient.GetAsync($"{baseRoute}/Get/{id}");
        }

        public Task<HttpResponseMessage> Post(long id, string name, DateTime date, TestPostRequest request)
        {
            return Post((object)id, name, date, request);
        }

        public Task<HttpResponseMessage> Post(object id, object name, object date, object? request)
        {
            var parameters = new Dictionary<string, string?>
            {
                { nameof(name), name.ToString() },
                { nameof(date), date.ToString() }
            };

            var url = $"{baseRoute}/Post/{id}" + QueryString.Create(parameters);

            return httpClient.PostAsJsonAsync(url, request);
        }
    }
}

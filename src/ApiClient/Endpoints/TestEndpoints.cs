
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

        public Task<HttpResponseMessage> Get(int id)
        {
            return Get((object)id);
        }

        public Task<HttpResponseMessage> Get(object id)
        {
            return httpClient.GetAsync($"{baseRoute}/Get/{id}");
        }
    }
}

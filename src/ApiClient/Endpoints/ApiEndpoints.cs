using Application.Models.Requests;
using System.Net.Http.Json;

namespace ApiClient.Endpoints
{
    public class ApiEndpoints(HttpClient httpClient, string apiType)
    {
        string BuildBasePath(int version = 1) => $"/api/v{version}/{apiType}";

        public Task<HttpResponseMessage> GetImage(long id)
        {
            return GetImage((object)id);
        }

        public Task<HttpResponseMessage> GetImage(object id)
        {
            return httpClient.GetAsync($"{BuildBasePath()}/Image/{id}");
        }

        public Task<HttpResponseMessage> GetImageGroup(long id)
        {
            return GetImageGroup((object)id);
        }

        public Task<HttpResponseMessage> GetImageGroup(object id)
        {
            return httpClient.GetAsync($"{BuildBasePath()}/ImageGroup/{id}")!;
        }

        public Task<HttpResponseMessage> SaveImageGroup(HttpContent? httpContent)
        {
            return httpClient.PostAsync($"{BuildBasePath()}/ImageGroup", httpContent);
        }

        public Task<HttpResponseMessage> SaveImageGroup(string imagePath)
        {
            var multipartContent = new MultipartFormDataContent();
            var byteArrayContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
            multipartContent.Add(byteArrayContent, "file", Path.GetFileName(imagePath));

            return SaveImageGroup(multipartContent);
        }

        public Task<HttpResponseMessage> DeleteImageGroup(long id, int version = 1)
        {
            return DeleteImageGroup((object)id, version);
        }

        public Task<HttpResponseMessage> DeleteImageGroup(object id, int version = 1)
        {
            return httpClient.DeleteAsync($"{BuildBasePath(version)}/ImageGroup/{id}");
        }

        public Task<HttpResponseMessage> Export(string tableName)
        {
            return httpClient.GetAsync($"{BuildBasePath()}/Export/{tableName}");
        }

        public Task<HttpResponseMessage> CreateCustomer(CreateCustomerRequest request)
        {
            return httpClient.PostAsJsonAsync($"{BuildBasePath()}/Customer", request);
        }
    }
}

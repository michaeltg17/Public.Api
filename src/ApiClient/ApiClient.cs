namespace ApiClient
{
    public class ApiClient(HttpClient httpClient)
    {
        public HttpClient HttpClient { get; } = httpClient;

        static string BuildBasePath(int version = 1) => $"api/v{version}";

        public Task<HttpResponseMessage> GetImage(long id)
        {
            return GetImage((object)id);
        }

        public Task<HttpResponseMessage> GetImage(object id)
        {
            return HttpClient.GetAsync($"{BuildBasePath()}/Image/{id}");
        }

        public Task<HttpResponseMessage> GetImageGroup(long id)
        {
            return GetImageGroup((object)id);
        }

        public Task<HttpResponseMessage> GetImageGroup(object id)
        {
            return HttpClient.GetAsync($"{BuildBasePath()}/ImageGroup/{id}")!;
        }

        public Task<HttpResponseMessage> SaveImageGroup(HttpContent? httpContent)
        {
            return HttpClient.PostAsync($"{BuildBasePath()}/ImageGroup", httpContent);
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
            return HttpClient.DeleteAsync($"{BuildBasePath(version)}/ImageGroup/{id}");
        }

        public Task<HttpResponseMessage> DeleteAllTestEntities()
        {
            return HttpClient.DeleteAsync($"Test/DeleteAllTestEntities");
        }

        public Task<HttpResponseMessage> ThrowInternalServerError()
        {
            return HttpClient.PostAsync($"Test/ThrowInternalServerError", null);
        }

        public Task<HttpResponseMessage> GetOk()
        {
            return HttpClient.GetAsync($"Test/GetOk");
        }

        public Task<HttpResponseMessage> Get(int id)
        {
            return Get((object)id);
        }

        public Task<HttpResponseMessage> Get(object id)
        {
            return HttpClient.GetAsync($"Test/Get/{id}");
        }

        public Task<HttpResponseMessage> RequestUnexistingRoute()
        {
            return HttpClient.GetAsync($"UnexistingRoute");
        }
    }
}

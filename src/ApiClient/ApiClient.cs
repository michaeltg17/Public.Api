namespace ApiClient
{
    public class ApiClient(HttpClient client)
    {
        public HttpClient HttpClient { get; } = client;

        public Task<HttpResponseMessage> GetImage(long id)
        {
            return GetImage((object)id);
        }

        public Task<HttpResponseMessage> GetImage(object id)
        {
            return HttpClient.GetAsync($"api/v1/Image/{id}");
        }

        public Task<HttpResponseMessage> GetImageGroup(long id)
        {
            return GetImageGroup((object)id);
        }

        public Task<HttpResponseMessage> GetImageGroup(object id)
        {
            return HttpClient.GetAsync($"api/v1/ImageGroup/{id}")!;
        }

        public Task<HttpResponseMessage> SaveImageGroup(HttpContent? httpContent)
        {
            return HttpClient.PostAsync("api/v1/ImageGroup", httpContent);
        }

        public Task<HttpResponseMessage> SaveImageGroup(string imagePath)
        {
            var multipartContent = new MultipartFormDataContent();
            var byteArrayContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
            multipartContent.Add(byteArrayContent, "file", Path.GetFileName(imagePath));

            return SaveImageGroup(multipartContent);
        }

        public Task<HttpResponseMessage> DeleteImageGroup(long id)
        {
            return DeleteImageGroup((object)id);
        }

        public Task<HttpResponseMessage> DeleteImageGroup(object id)
        {
            return HttpClient.DeleteAsync($"api/v1/ImageGroup/{id}");
        }

        public Task<HttpResponseMessage> DeleteAllTestEntities()
        {
            return HttpClient.DeleteAsync($"Test/DeleteAllTestEntities");
        }

        public Task<HttpResponseMessage> ThrowInternalServerError()
        {
            return HttpClient.PostAsync($"Test/ThrowInternalServerError", null);
        }
    }
}

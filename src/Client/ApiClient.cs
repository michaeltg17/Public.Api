namespace Client
{
    public class ApiClient(HttpClient client)
    {
        public HttpClient HttpClient { get; } = client;

        public Task<HttpResponseMessage> GetImage(long id)
        {
            return HttpClient.GetAsync($"Image/{id}");
        }

        public Task<HttpResponseMessage> GetImageGroup(long id)
        {
            return HttpClient.GetAsync($"ImageGroup/{id}")!;
        }

        public Task<HttpResponseMessage> SaveImageGroup(string imagePath)
        {
            var multipartContent = new MultipartFormDataContent();
            var byteArrayContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
            multipartContent.Add(byteArrayContent, "file", Path.GetFileName(imagePath));

            return HttpClient.PostAsync("ImageGroup", multipartContent);
        }

        public Task<HttpResponseMessage> DeleteImageGroup(long id)
        {
            return HttpClient.DeleteAsync($"ImageGroup/{id}");
        }
    }
}

using Domain.Models;
using Michael.Net.Extensions;
using System.Net.Http.Json;

namespace Client
{
    public class ApiClient
    {
        HttpClient client;

        public ApiClient(HttpClient client)
        {
            this.client = client;
        }

        public Task<Image?> GetImage(long id)
        {
            return client.GetFromJsonAsync<Image>($"GetImage?={id}");
        }

        public Task<ImageGroup?> GetImageGroup(long id)
        {
            return client.GetFromJsonAsync<ImageGroup>($"GetImageGroup?={id}");
        }

        public async Task<ImageGroup> SaveImageGroup(string imagePath)
        {
            var multipartContent = new MultipartFormDataContent();
            var byteArrayContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
            multipartContent.Add(byteArrayContent, "file", Path.GetFileName(imagePath));

            var response = await client.PostAsync("SaveImageGroup", multipartContent);
            return await response.FromJson<ImageGroup>();
        }

        public async Task DeleteImageGroup(long id)
        {
            await client.PostAsJsonAsync("DeleteImageGroup", id);
        }
    }
}

using Domain.Models;
using Michael.Net.Extensions;

namespace Client
{
    public class ApiClient
    {
        HttpClient client;

        public ApiClient(HttpClient client)
        {
            this.client = client;
        }

        Task<ImageGroup?> GetImage(int id)
        {
            var multipartContent = new MultipartFormDataContent();
            var byteArrayContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
            multipartContent.Add(byteArrayContent, "file", Path.GetFileName(imagePath));

            // When
            var response = await(await client.PostAsync("SaveImageGroup", multipartContent)).FromJson<ImageGroup>();
        }

        Task<ImageGroup?> GetImageGroup(int id)
        {
            var request = new RestRequest(Routes.GetImageGroup);
            request.AddParameter("id", id);
            return Client.GetAsync<ImageGroup>(request);
        }

        public async Task<ImageGroup> SaveImageGroup(string imagePath)
        {
            var multipartContent = new MultipartFormDataContent();
            var byteArrayContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
            multipartContent.Add(byteArrayContent, "file", Path.GetFileName(imagePath));

            var response = await client.PostAsync("SaveImageGroup", multipartContent);
            return await response.FromJson<ImageGroup>();
        }

        Task DeleteImageGroup(int id)
        {
            request = new RestRequest(Routes.DeleteImageGroup);
            request.AddParameter("id", response!.Id);
            await client.PostAsync(request);
        }
    }
}

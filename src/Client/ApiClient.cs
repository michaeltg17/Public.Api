using Domain.Models;
using Michael.Net.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;

namespace Client
{
    public class ApiClient
    {
        static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

        public HttpClient HttpClient { get; }

        public ApiClient(HttpClient client)
        {
            HttpClient = client;
        }

        async Task<T> Get(long id)
        {

        }

        public async Task<Image> GetImage(long id)
        {
            var response = await HttpClient.GetAsync($"Image/{id}");
            var content = await response.Content.ReadAsStreamAsync();

            if (response.IsSuccessStatusCode)
            {
                return (await JsonSerializer.DeserializeAsync<Image>(content, JsonSerializerOptions))!;
            }
            else
            {
                var problemDetails = (await JsonSerializer.DeserializeAsync<ProblemDetails>(content, JsonSerializerOptions))!;
                throw new ApiClientException(problemDetails);
            }
        }

        public Task<ImageGroup> GetImageGroup(long id)
        {
            return HttpClient.GetFromJsonAsync<ImageGroup>($"ImageGroup/{id}")!;
        }

        public async Task<ImageGroup> SaveImageGroup(string imagePath)
        {
            var multipartContent = new MultipartFormDataContent();
            var byteArrayContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
            multipartContent.Add(byteArrayContent, "file", Path.GetFileName(imagePath));

            var response = await HttpClient.PostAsync("ImageGroup", multipartContent);
            return await response.FromJson<ImageGroup>();
        }

        public async Task DeleteImageGroup(long id)
        {
            await HttpClient.DeleteAsync($"ImageGroup/{id}");
        }
    }
}

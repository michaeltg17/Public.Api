using Azure.Core;
using Azure;
using Domain.Models;
using RestSharp;

namespace FunctionalTests.Others
{
    public abstract class ApiTest
    {
        public RestClient Client { get; set; }

        public ApiTest()
        { 
            Client = new RestClient();
        }

        Task<ImageGroup?> GetImageGroup(int id) 
        {
            var request = new RestRequest(Routes.GetImageGroup);
            request.AddParameter("id", id);
            return Client.GetAsync<ImageGroup>(request);
        }

        Task<ImageGroup?> GetImageGroup(int id)
        {
            var request = new RestRequest(Routes.GetImageGroup);
            request.AddParameter("id", id);
            return Client.GetAsync<ImageGroup>(request);
        }

        Task<ImageGroup?> GetImageGroup(int id)
        {
            var request = new RestRequest(Routes.GetImageGroup);
            request.AddParameter("id", id);
            return Client.GetAsync<ImageGroup>(request);
        }

        Task DeleteImageGroup(int id)
        {
            request = new RestRequest(Routes.DeleteImageGroup);
            request.AddParameter("id", response!.Id);
            await client.PostAsync(request);
        }
    }

    public static class Routes
    {
        public const string ApiUrl = "http://localhost:80";
        public const string SaveImageGroup = ApiUrl + "/SaveImageGroup";
        public const string GetImageGroup = ApiUrl + "/GetImageGroup";
        public const string GetImage = ApiUrl + "/GetImage";
        public const string DeleteImageGroup = ApiUrl + "/DeleteImageGroup";
    }
}

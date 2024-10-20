using ApiClient.Converters;
using System.Text.Json;

namespace Core.Testing.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
        {
            Converters = { new NestedObjectConverter() }
        };

        public static async Task<HttpResponseMessage> EnsureSuccessStatusCode(this Task<HttpResponseMessage> responseTask)
        {
            var response = await responseTask;
            return response.EnsureSuccessStatusCode();
        }
    }
}

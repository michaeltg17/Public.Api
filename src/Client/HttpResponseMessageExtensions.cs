using Michael.Net.Exceptions;
using System.Text.Json;

namespace Client
{
    public static class HttpResponseMessageExtensions
    {
        static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
        {
            Converters = { new NestedObjectConverter() }
        };

        public static async Task<T> To<T>(this HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStreamAsync();
            var x = await response.Content.ReadAsStringAsync();
            var @object = await JsonSerializer.DeserializeAsync<T>(content, JsonSerializerOptions);
            return @object ?? throw new MichaelNetException("Deserialization from JSON failed. Result is null.");
        }

        public static async Task<T> To<T>(this Task<HttpResponseMessage> responseTask)
        {
            var response = await responseTask;
            return await response.To<T>();
        }
    }
}

using Michael.Net.Exceptions;
using System.Text.Json;

namespace Client
{
    public static class HttpResponseMessageExtensions
    {
        static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

        public static async Task<T> FromJson<T>(this HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStreamAsync();
            var @object = await JsonSerializer.DeserializeAsync<T>(content, JsonSerializerOptions);
            return @object ?? throw new MichaelNetException("Deserialization from JSON failed. Result is null.");
        }
    }
}

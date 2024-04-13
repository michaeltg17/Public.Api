using ApiClient.Converters;
using ApiClient.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ApiClient.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
        {
            Converters = { new NestedObjectConverter() }
        };

        public static async Task<T> To<T>(this HttpResponseMessage response)
        {
            var stream = await response.Content.ReadAsStreamAsync();

            try
            {
                var @object = await JsonSerializer.DeserializeAsync<T>(stream, JsonSerializerOptions);
                return @object ?? throw new ApiClientException("Deserialization from JSON failed. Result is null.");
            }
            catch (JsonException)
            {
                stream.Seek(0, SeekOrigin.Begin);
                var problemDetails = await JsonSerializer.DeserializeAsync<ProblemDetails>(stream, JsonSerializerOptions);
                throw new ApiException(problemDetails!);
            }
        }

        public static async Task<T> To<T>(this Task<HttpResponseMessage> responseTask)
        {
            var response = await responseTask;
            return await response.To<T>();
        }
    }
}

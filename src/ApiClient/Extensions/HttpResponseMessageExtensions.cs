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
            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content)) throw new ApiClientException("Response content is null, empty or whitespace.");

            try
            {
                return JsonSerializer.Deserialize<T>(content, JsonSerializerOptions)!;
            }
            catch (JsonException)
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(content, JsonSerializerOptions)!;
                throw new ApiException(problemDetails);
            }
        }

        public static async Task<T> To<T>(this Task<HttpResponseMessage> responseTask)
        {
            var response = await responseTask;
            return await response.To<T>();
        }
    }
}

namespace Core.Testing.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<HttpResponseMessage> EnsureSuccessStatusCode(this Task<HttpResponseMessage> responseTask)
        {
            var response = await responseTask;
            return response.EnsureSuccessStatusCode();
        }
    }
}

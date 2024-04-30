namespace Api.Middlewares
{
    public class SampleMiddleware(RequestDelegate next, ILogger<SampleMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            logger.LogInformation("{MiddlewareName} started.", nameof(SampleMiddleware));
            await next.Invoke(context);
            logger.LogInformation("{MiddlewareName} finished.", nameof(SampleMiddleware));
        }
    }
}

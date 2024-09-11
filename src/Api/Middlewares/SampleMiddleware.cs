using CrossCutting.Logging;

namespace Api.Middlewares
{
    public class SampleMiddleware(RequestDelegate next, ILogger<SampleMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            logger.LogMiddlewareStarted(nameof(SampleMiddleware));
            await next.Invoke(context);
            logger.LogMiddlewareFinished(nameof(SampleMiddleware));
        }
    }
}

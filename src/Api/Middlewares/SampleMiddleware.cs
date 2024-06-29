using CrossCutting.Logging;

namespace Api.Middlewares
{
    public partial class SampleMiddleware(RequestDelegate next, ILogger<SampleMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            Log.MiddlewareStarted(logger, nameof(SampleMiddleware));
            await next.Invoke(context);
            Log.MiddlewareFinished(logger, nameof(SampleMiddleware));
        }
    }
}

using CrossCutting.Logging;

namespace Api.Middlewares
{
    public partial class SampleMiddleware(RequestDelegate next, ILogger<SampleMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            MiddlewareStarted(logger, nameof(SampleMiddleware));
            await next.Invoke(context);
            MiddlewareFinished(logger, nameof(SampleMiddleware));
        }

        [LoggerMessage(
            EventId = LoggingEventIds.MiddlewareStarted, 
            EventName = nameof(MiddlewareStarted), 
            Level = LogLevel.Information,
            Message = "{middlewareName} started.",
            SkipEnabledCheck = true)]
        static partial void MiddlewareStarted(ILogger<SampleMiddleware> logger, string middlewareName);

        [LoggerMessage(
            EventId = LoggingEventIds.MiddlewareFinished,
            EventName = nameof(MiddlewareFinished),
            Level = LogLevel.Information,
            Message = "{middlewareName} finished.",
            SkipEnabledCheck = true)]
        static partial void MiddlewareFinished(ILogger<SampleMiddleware> logger, string middlewareName);
    }
}

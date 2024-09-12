using Microsoft.Extensions.Logging;

namespace CrossCutting.Logging
{
    public static partial class ILoggerExtensions
    {
        [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "{middlewareName} started.")]
        public static partial void LogMiddlewareStarted(this ILogger logger, string middlewareName);

        [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "{middlewareName} finished.")]
        public static partial void LogMiddlewareFinished(this ILogger logger, string middlewareName);

        [LoggerMessage(EventId = 3, Level = LogLevel.Information, Message = "{filterName} started on {actionName}.")]
        public static partial void LogFilterStarted(this ILogger logger, string filterName, string? actionName);

        [LoggerMessage(EventId = 4, Level = LogLevel.Information, Message = "{filterName} finished on {actionName}.")]
        public static partial void LogFilterFinished(this ILogger logger, string filterName, string? actionName);

        [LoggerMessage(EventId = 5, Level = LogLevel.Information, Message = "{route} - {httpMethod}")]
        public static partial void LogEndpoint(this ILogger logger, string route, string httpMethod);
    }
}

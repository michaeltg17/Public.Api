using Microsoft.Extensions.Logging;

namespace CrossCutting.Logging
{
    public static partial class Log
    {
        [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "{middlewareName} started.")]
        public static partial void MiddlewareStarted(ILogger logger, string middlewareName);

        [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "{middlewareName} finished.")]
        public static partial void MiddlewareFinished(ILogger logger, string middlewareName);

        [LoggerMessage(EventId = 3, Level = LogLevel.Information, Message = "{filterName} started on {actionName}.")]
        public static partial void FilterStarted(ILogger logger, string filterName, string? actionName);

        [LoggerMessage(EventId = 4, Level = LogLevel.Information, Message = "{filterName} finished on {actionName}.")]
        public static partial void FilterFinished(ILogger logger, string filterName, string? actionName);
    }
}

using Microsoft.AspNetCore.Mvc.Filters;
using CrossCutting.Logging;

namespace Api.Filters
{
    public class SampleFilter(ILogger<SampleFilter> logger) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            logger.LogFilterStarted(nameof(SampleFilter), context.ActionDescriptor.DisplayName);
            await next();
            logger.LogFilterFinished(nameof(SampleFilter), context.ActionDescriptor.DisplayName);
        }
    }
}

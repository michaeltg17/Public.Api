using Microsoft.AspNetCore.Mvc.Filters;
using CrossCutting.Logging;

namespace Api.Filters
{
    public class SampleFilter(ILogger<SampleFilter> logger) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Log.FilterStarted(logger, nameof(SampleFilter), context.ActionDescriptor.DisplayName);
            await next();
            Log.FilterFinished(logger, nameof(SampleFilter), context.ActionDescriptor.DisplayName);
        }
    }
}

using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class SampleFilter(ILogger<SampleFilter> logger) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            logger.LogInformation("{FilterName} started on {ActionName}.", nameof(SampleFilter), context.ActionDescriptor.DisplayName);
            await next();
            logger.LogInformation("{FilterName} finished on {ActionName}.", nameof(SampleFilter), context.ActionDescriptor.DisplayName);
        }
    }
}

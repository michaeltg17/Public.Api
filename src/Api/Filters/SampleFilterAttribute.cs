using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class SampleFilterAttribute() : ActionFilterAttribute, IFilterFactory
    {
        ILogger<SampleFilterAttribute> logger = default!;
        public bool IsReusable => true;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            logger = serviceProvider.GetRequiredService<ILogger<SampleFilterAttribute>>();
            return this;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            logger.LogInformation("{FilterName} started on {ActionName}.", nameof(SampleFilterAttribute), context.ActionDescriptor.DisplayName);
            await next();
            logger.LogInformation("{FilterName} finished on {ActionName}.", nameof(SampleFilterAttribute), context.ActionDescriptor.DisplayName);
        }
    }
}

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.Extensions.Options;
//using System.Net;

//namespace Api.Filters
//{
//    public class ValidationFilter(ILogger<SampleFilter> logger) : IEndpointFilter
//    {

//        //public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
//        //{
//        //    var obj = context.Arguments.FirstOrDefault(x => x?.GetType() == typeof(T)) as T;

//        //    if (obj is null)
//        //    {
//        //        return Results.BadRequest();
//        //    }

//        //    var validationResult = await _validator.ValidateAsync(obj);

//        //    if (!validationResult.IsValid)
//        //    {
//        //        return Results.BadRequest(string.Join("/n", validationResult.Errors));
//        //    }

//        //    return await next(context);
//        //}

//        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
//        {
//            var problemDetails = new ValidationProblemDetails(context.HttpContext.)
//            {
//                //Type = options.ClientErrorMapping[400].Link,
//                Title = "ValidationException",
//                Status = (int)HttpStatusCode.BadRequest,
//                Detail = "Please check the errors property for additional details.",
//                Instance = context.HttpContext.Request.Path
//            };

//            return Results.BadRequest(problemDetails);
//        }

//        //public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//        //{
//        //    var problemDetails = new ValidationProblemDetails(context.ModelState)
//        //    {
//        //        //Type = options.ClientErrorMapping[400].Link,
//        //        Title = "ValidationException",
//        //        Status = (int)HttpStatusCode.BadRequest,
//        //        Detail = "Please check the errors property for additional details.",
//        //        Instance = context.HttpContext.Request.Path
//        //    };

//        //    return new BadRequestObjectResult(problemDetails);
//        //    await next();
//        //    Log.FilterFinished(logger, nameof(SampleFilter), context.ActionDescriptor.DisplayName);
//        //}
//    }
//}

using Application.Exceptions;
using Common.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using System.Net;

namespace Api.Extensions
{
    public static class ExceptionHandlerExtensions
    {
        public static WebApplication UseExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(config => config.Run(async httpContext =>
            {
                httpContext.Response.ContentType = "application/problem+json";
                var problemDetailsService = httpContext.RequestServices.GetRequiredService<IProblemDetailsService>();

                var exceptionHandlerFeature = httpContext.Features.GetRequiredFeature<IExceptionHandlerFeature>();
                var exception = exceptionHandlerFeature.Error;

                httpContext.Response.StatusCode = exception switch
                {
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    ApiException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError,
                };

                var problemDetailsContext = BuildProblemDetailsContext(exception, httpContext);

                await problemDetailsService.WriteAsync(problemDetailsContext);
            }));

            return app;
        }

        static ProblemDetailsContext BuildProblemDetailsContext(Exception exception, HttpContext httpContext)
        {
            var isInternalServerError = httpContext.Response.StatusCode == (int)HttpStatusCode.InternalServerError;

            return new ProblemDetailsContext
            {
                Exception = isInternalServerError ? null : exception,
                HttpContext = httpContext,
                ProblemDetails =
                {
                    Title =  isInternalServerError ? "InternalServerError" : exception!.GetType().GetNameWithoutGenericArity(),
                    Detail = isInternalServerError ? "Internal server error. Please contact the API support." : exception!.Message,
                    Status = httpContext.Response.StatusCode,
                    Instance = httpContext.Request.Path
                }
            };
        }
    }
}

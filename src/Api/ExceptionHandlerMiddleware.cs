using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api
{
    public class ExceptionHandlerMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
                throw;
            }
        }

        static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var problemDetails = new ProblemDetails() { Title = exception.Message };

            switch (exception)
            {
                case NotFoundException:
                    problemDetails.Status = (int)HttpStatusCode.NotFound;
                    break;
                case ApiException:
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    return;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = problemDetails.Status.Value;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}

using Api.Filters;
using Api.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints
{
    public static class TestEndpoints
    {
        public static WebApplication MapTestEndpoints(this WebApplication webApplication, IEndpointRouteBuilder builder)
        {
            var testEndpoints = builder.MapGroup("/TestMinimalApi");

            testEndpoints
                .MapGet("get/{id}", (long id, CancellationToken cancellationToken) => Task.CompletedTask)
                .WithName("TestMinimalApi.Get")
                .WithOpenApi()
                .AddEndpointFilter<ValidationFilter>();

            testEndpoints
                .MapPost("ThrowInternalServerError", _ => throw new Exception("Sensitive data"))
                .WithName("TestMinimalApi.ThrowInternalServerError")
                .WithOpenApi();

            testEndpoints
                .MapPost("post/{id}", 
                    (long id, 
                    [FromQuery] string name, 
                    [FromQuery] DateTime date, 
                    [FromBody] TestPostRequest request, 
                    CancellationToken cancellationToken) => Task.CompletedTask)
                .WithName("TestMinimalApi.Post")
                .WithOpenApi()
                .AddEndpointFilter<ValidationFilter>();

            return webApplication;
        }
    }
}

using Api.Filters;

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

            return webApplication;
        }
    }
}

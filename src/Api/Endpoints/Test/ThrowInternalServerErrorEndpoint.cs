using Api.Extensions;

namespace Api.Endpoints.Test
{
    public static class ThrowInternalServerErrorEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app
                .MapPost("ThrowInternalServerError", _ => throw new Exception("Sensitive data"))
                .WithTestMinimalApiName("ThrowInternalServerError")
                .WithOpenApi();
        }
    }
}

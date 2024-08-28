using Api.Abstractions;

namespace Api.Endpoints.Test
{
    public class ThrowInternalServerErrorEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app
                .MapPost("ThrowInternalServerError", _ => throw new Exception("Sensitive data"))
                .WithName("TestMinimalApi.ThrowInternalServerError")
                .WithOpenApi();
        }
    }
}

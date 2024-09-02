using Api.Extensions;
using Api.Filters;

namespace Api.Endpoints.Test
{
    public static class GetOkEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapGet("Get", (
                CancellationToken cancellationToken) =>
            {
                return Task.CompletedTask;
            })
            .WithTestMinimalApiName("GetOk")
            .WithOpenApi()
            .AddEndpointFilter<ValidationFilter>();
        }
    }
}

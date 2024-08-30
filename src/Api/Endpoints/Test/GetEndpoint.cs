using Api.Extensions;
using Api.Filters;

namespace Api.Endpoints.Test
{
    public static class GetEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app
                .MapGet("Get/{id}", (long id, CancellationToken cancellationToken) => Task.CompletedTask)
                .WithTestMinimalApiName("Get")
                .WithOpenApi()
                .AddEndpointFilter<ValidationFilter>();
        }
    }
}

using Api.Abstractions;
using Api.Filters;

namespace Api.Endpoints.Test
{
    public class GetEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app
                .MapGet("TestMinimalApi/Get/{id}", (long id, CancellationToken cancellationToken) => Task.CompletedTask)
                .WithName("TestMinimalApi.Get")
                .WithOpenApi()
                .AddEndpointFilter<ValidationFilter>();
        }
    }
}

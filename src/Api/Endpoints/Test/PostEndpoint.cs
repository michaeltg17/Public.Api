using Api.Extensions;
using Api.Filters;
using Api.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Test
{
    public static class PostEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapPost("Post/{id}", (
                long id,
                [FromQuery] string name,
                [FromQuery] DateTime date,
                [FromBody] TestPostRequest request,
                CancellationToken cancellationToken) => 
            {
                return Task.CompletedTask;
            })
            .WithTestMinimalApiName("Post")
            .WithOpenApi()
            .AddEndpointFilter<ValidationFilter>();
        }
    }
}

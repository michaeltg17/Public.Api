using Api.Abstractions;
using Api.Filters;
using Api.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Test
{
    public class PostEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app
                .MapPost("Post/{id}",
                    (long id,
                    [FromQuery] string name,
                    [FromQuery] DateTime date,
                    [FromBody] TestPostRequest request,
                    CancellationToken cancellationToken) => Task.CompletedTask)
                .WithName("TestMinimalApi.Post")
                .WithOpenApi()
                .AddEndpointFilter<ValidationFilter>();
        }
    }
}

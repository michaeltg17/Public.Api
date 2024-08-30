using Api.Abstractions;
using Application.Services;

namespace Api.Endpoints
{
    public class GetImageEndpoint : IVersionedEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("Image/{id}", async (
                ImageService imageService,
                long id,
                CancellationToken cancellationToken) =>
            {
                await imageService.GetImage(id, cancellationToken);
            })
            .WithName("GetImage")
            .WithOpenApi();
        }
    }
}

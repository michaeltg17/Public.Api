using Application.Services;
using Asp.Versioning.Builder;

namespace Api.Endpoints
{
    public static class ImageEndpoints
    {
        public static WebApplication AddImageEndpoints(this WebApplication webApplication, IVersionedEndpointRouteBuilder builder)
        {
            var imageEndpoints = builder.MapGroup("/api/v{version:apiVersion}/image");

            imageEndpoints
                .MapGet("{id}",
                    async (ImageService imageService,
                    long id,
                    CancellationToken cancellationToken)
                    => await imageService.GetImage(id, cancellationToken))
                .WithName("GetImage")
                .WithOpenApi();

            return webApplication;
        }
    }
}

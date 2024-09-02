using Api.Extensions;
using Application.Services;

namespace Api.Endpoints.ImageGroup
{
    public static class GetImageGroupEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapGet("ImageGroup/{id}", (
                ImageService imageService,
                long id,
                CancellationToken cancellationToken) =>
            {
                return imageService.GetImageGroup(id, cancellationToken);
            })
            .WithMinimalApiName("GetImageGroup")
            .WithOpenApi();
        }
    }
}

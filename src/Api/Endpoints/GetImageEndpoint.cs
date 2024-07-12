using Application.Services;

namespace Api.Endpoints
{
    public static class GetImageEndpoint
    {
        public static WebApplication AddGetImageEndpoint(this WebApplication webApplication)
        {
            webApplication.MapGet("{id}",
                (ImageService imageService,
                long id,
                CancellationToken cancellationToken)
                => imageService.GetImage(id, cancellationToken));

            return webApplication;
        }
    }
}

using Asp.Versioning;
using Api.Endpoints.Test;
using Api.Endpoints.Image;
using Api.Endpoints.ImageGroup;

namespace Api.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var v1 = app.MapGroupWithVersion(1);
        GetImageEndpoint.Map(v1);
        GetImageGroupEndpoint.Map(v1);
        SaveImageGroupEndpoint.Map(v1);
        DeleteImageGroupEndpoint.Map(v1);
        ExportEndpoint.Map(v1);

        var v2 = app.MapGroupWithVersion(2);
        DeleteImageGroupV2Endpoint.Map(v2);

        var test = app.MapGroup("TestMinimalApi");
        GetEndpoint.Map(test);
        GetOkEndpoint.Map(test);
        PostEndpoint.Map(test);
        ThrowInternalServerErrorEndpoint.Map(test);

        return app;
    }

    static RouteGroupBuilder MapGroupWithVersion(this IEndpointRouteBuilder app, int version)
    {
        var apiVersionSet = app
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(version))
            .ReportApiVersions()
            .Build();

        return app
            .MapGroup("api/v{version:apiVersion}/MinimalApi")
            .WithApiVersionSet(apiVersionSet);
    }
}

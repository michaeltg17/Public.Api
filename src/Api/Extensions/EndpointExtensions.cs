using Asp.Versioning;
using Api.Endpoints.Test;
using Api.Endpoints.Image;
using Api.Endpoints.ImageGroup;

namespace Api.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var v1Group = app.MapGroupWithVersion(1);
        GetImageEndpoint.Map(v1Group);
        GetImageGroupEndpoint.Map(v1Group);
        SaveImageGroupEndpoint.Map(v1Group);
        DeleteImageGroupEndpoint.Map(v1Group);

        var v2Group = app.MapGroupWithVersion(2);
        DeleteImageGroupV2Endpoint.Map(v2Group);

        var testGroup = app.MapGroup("TestMinimalApi");
        GetEndpoint.Map(testGroup);
        GetOkEndpoint.Map(testGroup);
        PostEndpoint.Map(testGroup);
        ThrowInternalServerErrorEndpoint.Map(testGroup);

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

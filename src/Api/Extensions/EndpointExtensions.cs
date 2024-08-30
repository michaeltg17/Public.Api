using Asp.Versioning;
using Api.Endpoints.Test;
using Api.Endpoints;

namespace Api.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var apiVersionSet = app
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();
        var versionedGroup = app
            .MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);

        GetImageEndpoint.Map(versionedGroup);


        var testGroup = app.MapGroup("TestMinimalApi");

        GetEndpoint.Map(testGroup);
        PostEndpoint.Map(testGroup);
        ThrowInternalServerErrorEndpoint.Map(testGroup);

        return app;
    }
}

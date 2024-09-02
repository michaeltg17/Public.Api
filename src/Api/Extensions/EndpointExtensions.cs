using Asp.Versioning;
using Api.Endpoints.Test;
using Api.Endpoints.Image;

namespace Api.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var v1Version = app
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();
        var v1Group = app
            .MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(v1Version);

        GetImageEndpoint.Map(v1Group);

        var testGroup = app.MapGroup("TestMinimalApi");

        GetEndpoint.Map(testGroup);
        PostEndpoint.Map(testGroup);
        ThrowInternalServerErrorEndpoint.Map(testGroup);

        return app;
    }

    static IEndpointRouteBuilder GetFromVersion(this IEndpointRouteBuilder  int version)
    {
        var apiVersionSet = app
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        return app
            .MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(v1Version);
    }
}

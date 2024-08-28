using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Api.Abstractions;

namespace Api.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint<IEndpointRouteBuilder>), type))
            .ToArray();

        //Extension for add single or throw
        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints<T>(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null) where T : IEndpointRouteBuilder
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<T>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (T endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
}

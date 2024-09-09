namespace Api.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication LogEndpoints(this WebApplication app)
        {
            app.Lifetime.ApplicationStarted.Register(() =>
            {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                var endpoints = app.Services.GetRequiredService<EndpointDataSource>().Endpoints.Cast<RouteEndpoint>();

                foreach (var endpoint in endpoints)
                {
                    var httpMethod = endpoint.Metadata.GetRequiredMetadata<HttpMethodMetadata>().HttpMethods[0];
                    var route = endpoint.RoutePattern.RawText;
                    logger.LogInformation($"{{route}} - {{httpMethod}}", route, httpMethod);
                }
            });

            return app;
        }
    }
}

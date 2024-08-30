namespace Api.Extensions
{
    public static class RoutingEndpointConventionBuilderExtensions
    {
        public static TBuilder WithTestMinimalApiName<TBuilder>(this TBuilder builder, string endpointName) 
            where TBuilder : IEndpointConventionBuilder
        {
            builder.WithName("TestMinimalApi." + endpointName);
            return builder;
        }
    }
}

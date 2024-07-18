namespace Api.Endpoints
{
    public static class TestEndpoints
    {
        public static WebApplication AddTestEndpoints(this WebApplication webApplication, IEndpointRouteBuilder builder)
        {
            var testEndpoints = builder.MapGroup("/TestMinimalApi");

            testEndpoints
                .MapGet("get/{id}", (long id, CancellationToken cancellationToken) => Task.CompletedTask)
                .WithName("TestMinimalApi.Get")
                .WithOpenApi();

            return webApplication;
        }
    }
}

using Api.Extensions;
using Application.Models.Requests;
using Application.Services;

namespace Api.Endpoints.ImageGroup
{
    public static class CreateCustomerEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapPost("Customer", async (
                CreateCustomerRequest request,
                CustomerService customerService,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var customerId = await customerService.CreateCustomer(request);

                var locationUri = $"/Customer/{customerId}";
                context.Response.Headers.Location = locationUri;
                return Results.Created(locationUri, customerId);
            })
            .WithMinimalApiName("Customer")
            .WithOpenApi()
            .DisableAntiforgery();
        }
    }
}

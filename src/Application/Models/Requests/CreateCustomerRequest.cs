namespace Application.Models.Requests
{
    public record CreateCustomerRequest(
        string FirstName,
        string LastName,
        string AddressLine,
        string City,
        string State,
        string PostalCode,
        string Country,
        string Email,
        string PhoneNumber
    );
}

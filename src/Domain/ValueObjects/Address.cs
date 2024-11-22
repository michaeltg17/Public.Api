namespace Domain.ValueObjects
{
    public readonly record struct Address(
        string FirstName,
        string LastName,
        string Street,
        string PostalCode,
        string City, 
        string State,
        string Country);
}
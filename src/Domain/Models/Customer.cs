namespace Domain.Models
{
    public class Customer : Entity
    {
        public required string Name { get; init; }
        public required string Address { get; init; }
        public required string Email { get; init; }
        public required string Phone { get; init; }
    }
}

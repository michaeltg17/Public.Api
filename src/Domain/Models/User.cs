namespace Domain.Models
{
    public class User : Entity
    {
        public required string Name { get; init; }
        public required string Email { get; init; }
    }
}

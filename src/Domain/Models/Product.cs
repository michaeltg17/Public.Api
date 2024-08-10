namespace Domain.Models
{
    public class Product : Entity
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public decimal Price { get; init; }
    }
}

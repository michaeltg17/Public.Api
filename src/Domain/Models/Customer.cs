using System.Runtime.Serialization;

namespace Domain.Models
{
    public class Customer : Entity
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }

        public string? TestProperty { get; set; }
    }
}

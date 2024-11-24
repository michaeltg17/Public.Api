using Domain.ValueObjects;

namespace Domain.Models
{
    public class Customer : Entity
    {
        public required Name FirstName { get; set; }
        public required Name LastName { get; set; }
        public required Address Address { get; set; }
        public required Email Email { get; set; }
        public required Phone Phone { get; set; }
    }
}

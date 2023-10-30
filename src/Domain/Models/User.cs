namespace Domain.Models
{
    public class User : Entity
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}

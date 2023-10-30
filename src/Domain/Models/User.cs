namespace Domain.Models
{
    public class User : Entity
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;

        public virtual User CreatedByNavigation { get; set; } = default!;
        public virtual ICollection<User> InverseCreatedByNavigation { get; set; } = new List<User>();
    }
}

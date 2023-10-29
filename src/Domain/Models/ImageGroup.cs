namespace Domain.Models
{
    public class ImageGroup : Entity
    {
        public string Name { get; set; } = default!;
        public virtual IEnumerable<Image> Images { get; set; } = new List<Image>();
    }
}

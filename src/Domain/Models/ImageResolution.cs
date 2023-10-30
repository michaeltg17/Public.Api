namespace Domain.Models
{
    public class ImageResolution : Entity
    {
        public string Name { get; set; } = default!;

        public virtual IEnumerable<Image> Images { get; set; } = default!;
    }
}

namespace Domain.Models
{
    public class ImageResolution : Entity
    {
        public required string Name { get; init; }

        public virtual IEnumerable<Image> ImagesNavigation { get; set; } = default!;
    }
}

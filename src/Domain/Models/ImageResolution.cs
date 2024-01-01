namespace Domain.Models
{
    public class ImageResolution : Entity
    {
        public string Name { get; set; } = default!;

        public virtual IEnumerable<Image> ImagesNavigation { get; set; } = default!;
    }
}

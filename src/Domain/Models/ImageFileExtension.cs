namespace Domain.Models
{
    public class ImageFileExtension : Entity
    {
        public long ImageType { get; init; }
        public required string FileExtension { get; init; }

        public virtual ImageType ImageTypeNavigation { get; set; } = default!;
    }
}

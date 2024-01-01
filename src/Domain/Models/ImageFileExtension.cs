namespace Domain.Models
{
    public class ImageFileExtension : Entity
    {
        public long ImageType { get; set; }
        public string FileExtension { get; set; } = default!;

        public virtual ImageType ImageTypeNavigation { get; set; } = default!;
    }
}

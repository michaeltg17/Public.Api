namespace Domain.Models
{
    public class ImageType : Entity
    {
        public required string Abbreviation { get; init; }
        public required string Name { get; init; }

        public virtual IEnumerable<ImageFileExtension> FileExtensionNavigation { get; set; } = default!;
        public virtual IEnumerable<ImageGroup> ImageGroupNavigation { get; set; } = default!;

        public string GetDefaultFileExtension() => FileExtensionNavigation.First().FileExtension;
    }
}

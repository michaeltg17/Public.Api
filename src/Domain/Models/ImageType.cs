namespace Domain.Models
{
    public class ImageType : Entity
    {
        public string Abbreviation { get; set; } = default!;
        public string Name { get; set; } = default!;

        public virtual IEnumerable<ImageFileExtension> FileExtensionNavigation { get; set; } = default!;
        public virtual IEnumerable<ImageGroup> ImageGroupNavigation { get; set; } = default!;

        public string GetDefaultFileExtension() => FileExtensionNavigation.First().FileExtension;
    }
}

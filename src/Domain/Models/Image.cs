namespace Domain.Models
{
    public class Image : Entity
    {
        public string Url { get; set; } = default!;
        public long Resolution { get; set; }
        public long Group { get; set; }

        public virtual ImageResolution ResolutionNavigation { get; set; } = default!;
        public virtual ImageGroup GroupNavigation { get; set; } = default!;

        public string FileName => Guid + "." + GroupNavigation.TypeNavigation.GetDefaultFileExtension();
    }
}

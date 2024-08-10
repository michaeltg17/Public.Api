namespace Domain.Models
{
    public class Image : Entity
    {
        public required string Url { get; init; }
        public long Resolution { get; init; }
        public long Group { get; init; }

        public virtual ImageResolution ResolutionNavigation { get; set; } = default!;
        public virtual ImageGroup GroupNavigation { get; set; } = default!;

        public string FileName => Guid + "." + GroupNavigation!.TypeNavigation!.GetDefaultFileExtension();
    }
}

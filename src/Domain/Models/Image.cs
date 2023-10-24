namespace Domain.Models
{
    public class Image : Entity
    {
        public string Url { get; set; } = default!;
        public long ImageResolutionId {get; set;}
        public ImageResolution ImageResolution { get; set; } = default!;
        public long ImageGroupId { get; set; }
        public ImageGroup ImageGroup { get; set; } = default!;
    }
}

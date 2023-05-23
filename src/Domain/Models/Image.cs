using Dapper.Contrib.Extensions;

namespace Domain.Models
{
    public class Image : Entity
    {
        public string Url { get; set; } = default!;
        public long ImageResolutionId {get; set;}
        [Write(false)] public ImageResolution ImageResolution { get; set; } = default!;
        public long ImageGroupId { get; set; }
        [Write(false)] public ImageGroup ImageGroup { get; set; } = default!;
    }
}

using Dapper.Contrib.Extensions;

namespace Domain.Models
{
    public class ImageGroup : Entity
    {
        public string Name { get; set; } = default!;
        [Write(false)] public List<Image> Images { get; set; } = default!;
    }
}

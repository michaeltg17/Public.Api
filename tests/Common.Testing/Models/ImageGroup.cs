namespace Common.Testing.Models
{
    public class ImageGroup : Entity
    {
        public required string Name { get; set; }
        public required IEnumerable<Image> Images { get; set; }
    }
}

namespace Core.Testing.Models
{
    public class Image : Entity
    {
        public required string Url { get; set; }
        public long Resolution { get; set; }
        public long Group { get; set; }
        public required string FileName { get; set; }
    }
}

namespace Common.Testing.Models
{
    public class Image : Entity
    {
        public string Url { get; set; } = default!;
        public long Resolution { get; set; }
        public long Group { get; set; }
        public string FileName { get; set; } = default!;
    }
}

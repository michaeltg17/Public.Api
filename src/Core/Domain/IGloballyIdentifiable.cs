namespace Core.Domain
{
    public interface IGloballyIdentifiable
    {
        public Guid Guid { get; set; }
    }
}

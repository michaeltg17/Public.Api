namespace Core
{
    public interface IFactory<T>
    {
        Task<T> Create();
    }
}

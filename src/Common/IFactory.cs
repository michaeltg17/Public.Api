namespace Common
{
    public interface IFactory<T>
    {
        Task<T> Create();
    }
}

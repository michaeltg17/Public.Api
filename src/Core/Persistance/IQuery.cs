using System.Data;

namespace Core.Persistence
{
    public interface IQuery<T>
    {
        public Task<T> Execute(IDbConnection connection);
    }
}

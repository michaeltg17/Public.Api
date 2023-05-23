using Michael.Net.Domain;
using Michael.Net.Extensions;
using Michael.Net.Persistence.Exceptions;
using Dapper.Contrib.Extensions;
using Michael.Net.Persistence;

namespace Persistence
{
    public class Repository : IRepository
    {
        readonly IUnitOfWork unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork.ThrowIfNull();
        }

        public async Task<T?> Get<T>(int id) where T : class, IEntity, new()
        {
            return await unitOfWork.Connection.GetAsync<T>(id);
        }

        public async Task<T> GetOrThrow<T>(int id) where T : class, IEntity, new()
        {
            return await Get<T>(id) ?? throw new EntityNotFoundException<T>(id);
        }

        public async Task<IEnumerable<T>> Get<T>() where T : class, IEntity, new()
        {
            return await unitOfWork.Connection.GetAllAsync<T>();
        }

        public async Task<T> Get<T>(IQuery<T> query) where T : class, IEntity, new()
        {
            return await query.Execute(unitOfWork.Connection);
        }

        public async Task Insert<T>(T entity) where T : class, IEntity, new()
        {
            await unitOfWork.Connection.InsertAsync(entity);
        }

        public async Task Update<T>(T entity) where T : class, IEntity, new()
        {
            await unitOfWork.Connection.UpdateAsync(entity);
        }

        public async Task Delete<T>(int id) where T : class, IEntity, new()
        {
            await unitOfWork.Connection.DeleteAsync(new T() { Id = id });
        }

        public async Task Delete<T>(T entity) where T : class, IEntity, new()
        {
            await unitOfWork.Connection.DeleteAsync(entity);
        }
    }
}

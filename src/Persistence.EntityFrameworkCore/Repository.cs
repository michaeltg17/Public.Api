using Michael.Net.Domain;
using Michael.Net.Persistance;
using Michael.Net.Persistance.Exceptions;

namespace Persistance.EntityFrameworkCore
{
    public class Repository : IRepository
    {
        readonly DbContext dbContext;

        public Repository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<T?> GetById<T>(int id) where T : class, IEntity, new()
        {
            return await dbContext.FindAsync<T>(id).AsTask();
        }

        public async Task<T> GetByIdOrThrow<T>(int id) where T : class, IEntity, new()
        {
            return await GetById<T>(id) ?? throw new EntityNotFoundException<T>(id);
        }

        public async Task Insert<T>(T entity) where T : class, IEntity, new()
        {
            await dbContext.AddAsync(entity).AsTask();
        }

        public Task Update<T>(T entity) where T : class, IEntity, new()
        {
            return Task.FromResult(dbContext.Update(entity));
        }

        public Task Delete<T>(int id) where T : class, IEntity, new()
        {
            var entity = new T() { Id = id };
            var entry = dbContext.Entry(entity);
            return Task.FromResult(dbContext.Remove(entry));
        }

        Task<IEnumerable<T>> IRepository.Get<T>(IQuery query)
        {
            throw new NotImplementedException();
        }
    }
}

using Michael.Net.Persistance;

namespace Persistance.EntityFrameworkCore
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IRepository Repository { get; }

        readonly DbContext dbContext;

        public UnitOfWork(
            DbContext dbContext,
            IRepository repository)
        {
            this.dbContext = dbContext;
            Repository = repository;
        }

        public async Task SaveChanges()
        {
            await dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

using System.Data;
using System.Transactions;

namespace Persistence
{
    public interface IUnitOfWork
    {
        public IDbConnection Connection { get; }

        public Task<TransactionScope> BeginTransaction();
    }
}

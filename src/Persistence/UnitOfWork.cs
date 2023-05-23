using Application;
using Dapper.Logging;
using Michael.Net.Extensions;
using System.Data;
using System.Transactions;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        IDbConnection? connection;
        public IDbConnection Connection
        {
            get
            {
                return connection ??= connectionFactory.CreateConnection();
            }
            set
            {
                connection = value;
            }
        }

        readonly IDbConnectionFactory connectionFactory;

        public UnitOfWork(
            IDbConnectionFactory connectionFactory,
            ISettings settings)
        {
            this.connectionFactory = connectionFactory.ThrowIfNull();
            settings.ThrowIfNull();
        }

        public Task<TransactionScope> BeginTransaction()
        {
            var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            Connection = connectionFactory.CreateConnection();

            return Task.FromResult(transactionScope);
        }
    }
}

using EntityFramework.UnitOfWork.Interfaces;
using System.Data;

namespace EntityFramework.UnitOfWork
{
    internal sealed class UnitOfWorkTransactionHandler<TConnection> : UnitOfWorkHandler<TConnection>, IUowTransactionHandler where TConnection : IDbConnection
    {
        private readonly IDbTransaction _transaction;
        private bool _commited = false;

        internal UnitOfWorkTransactionHandler(IEnumerable<Type> repositoryTypes, string connectionString) 
            : base(repositoryTypes, connectionString)
        {
            _transaction = Connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
            _commited = true;
        }

        public async Task CommitAsync()
        {
            _transaction.Commit();
            _commited = await Task.Run(() => { return true; });
        }

        private protected override void DisposeOverrides()
        {
            if (!_commited)
            {
                _transaction.Rollback();
            }

            _transaction.Dispose();
        }
    }
}

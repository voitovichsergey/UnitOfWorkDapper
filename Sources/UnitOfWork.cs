using Dapper.UnitOfWork.Interfaces;
using System.Data;

namespace Dapper.UnitOfWork
{
    public sealed class UnitOfWork<TConnection> : IUnitOfWork where TConnection : IDbConnection
    {
        private readonly IEnumerable<Type> _repositoryTypes;
        private readonly string _connectionString;

        public UnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
            _repositoryTypes = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(t => !t.IsAbstract && !t.IsInterface && t.GetInterfaces().Any(i => i.Equals(typeof(IUowRepository))));
        }

        public IUowTransactionHandler StartTransaction()
        {
            return new UnitOfWorkTransactionHandler<TConnection>(_repositoryTypes,
                _connectionString);
        }

        public IUowHandler Selector()
        {
            return new UnitOfWorkHandler<TConnection>(_repositoryTypes,
                _connectionString);
        }
    }
}

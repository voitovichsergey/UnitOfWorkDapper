using Dapper.UnitOfWork.Interfaces;
using System.Data;

namespace Dapper.UnitOfWork
{
    internal class UnitOfWorkHandler<TConnection> : IUowHandler where TConnection : IDbConnection
    {
        private protected readonly IDbConnection Connection;
        private readonly Dictionary<string, IUowRepository> _repositories = [];

        internal UnitOfWorkHandler(IEnumerable<Type> repositoryTypes, string connectionString)
        {
            Connection = (IDbConnection)Activator.CreateInstance(typeof(TConnection))
                ?? throw new Exception("Unable to create DbConnection");
            Connection.ConnectionString = connectionString;
            Connection.Open();

            _repositories = repositoryTypes
                .ToDictionary(t => t
                                .GetInterfaces()
                                .Where(i => !i.Equals(typeof(IUowRepository)))
                                .FirstOrDefault(x => x.GetInterfaces().Contains(typeof(IUowRepository)))?
                                .Name ?? string.Empty,
                              t => (IUowRepository)(Activator.CreateInstance(t, Connection) ?? new object()));
        }

        public T Repository<T>() where T : IUowRepository
        {
            return (T)_repositories[typeof(T).Name];
        }

        public void Dispose()
        {
            DisposeOverrides();
            _repositories.Clear();
            Connection.Dispose();
        }

        private protected virtual void DisposeOverrides()
        {
        }
    }
}

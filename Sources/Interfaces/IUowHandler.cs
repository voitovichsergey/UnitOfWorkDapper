namespace EntityFramework.UnitOfWork.Interfaces
{
    public interface IUowHandler : IDisposable
    {
        /// <summary>
        /// Get the repository inherited from the interface <see cref="IUowRepository"/>.
        /// </summary>
        /// <typeparam name="T">Repository interface type.</typeparam>
        /// <returns>Implementation of repository interface.</returns>
        public T Repository<T>() where T : IUowRepository;
    }
}

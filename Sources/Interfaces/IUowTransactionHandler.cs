namespace EntityFramework.UnitOfWork.Interfaces
{
    public interface IUowTransactionHandler : IUowHandler
    {
        /// <summary>
        /// Commit the transaction.
        /// If the transaction has not been commited, a rollback will be performed.
        /// </summary>
        public void Commit();

        /// <summary>
        /// Commit the transaction.
        /// If the transaction has not been commited, a rollback will be performed.
        /// </summary>
        public Task CommitAsync();
    }
}

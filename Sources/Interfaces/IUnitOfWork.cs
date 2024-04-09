namespace Dapper.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        public IUowTransactionHandler StartTransaction();

        public IUowHandler Selector();
    }
}

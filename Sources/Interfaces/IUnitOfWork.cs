namespace EntityFramework.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        public IUowTransactionHandler StartTransaction();

        public IUowHandler Selector();
    }
}

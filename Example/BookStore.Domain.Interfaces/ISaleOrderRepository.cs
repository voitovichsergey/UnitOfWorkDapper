using EntityFramework.UnitOfWork.Interfaces;

namespace BookStore.Domain.Interfaces
{
    public interface ISaleOrderRepository : IUowRepository
    {
        public Task SellAsync(long bookId);
    }
}

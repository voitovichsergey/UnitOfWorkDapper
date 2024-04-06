using BookStore.Domain.Interfaces;
using BookStore.Services.Interfaces;
using EntityFramework.UnitOfWork.Interfaces;

namespace BookStore.Infrastructure.Business
{
    public sealed class SaleOrderService(IUnitOfWork unitOfWork) : ISaleOrderService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Sale(long bookId)
        {
            using (var uow = _unitOfWork.StartTransaction())
            {
                await uow.Repository<ISaleOrderRepository>().SellAsync(bookId);
                await uow.Repository<IBookRepository>().AppendAsync(bookId, -1);

                var book = await uow.Repository<IBookRepository>().GetAsync(bookId);
                if (book.Count < 0)
                    throw new InvalidOperationException("No books for sale");

                await uow.CommitAsync();
            }
        }
    }
}

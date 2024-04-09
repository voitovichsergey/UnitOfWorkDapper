using BookStore.Domain.Core;
using Dapper.UnitOfWork.Interfaces;

namespace BookStore.Domain.Interfaces
{
    public interface IBookRepository : IUowRepository
    {
        public Task<Book> GetAsync(long bookId);

        public Task<Book[]> GetAsync();

        public Task<long> CreateAsync(Book book);

        public Task AppendAsync(long bookId, int count);
    }
}

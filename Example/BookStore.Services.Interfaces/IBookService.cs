using BookStore.Services.Interfaces.Dto;

namespace BookStore.Services.Interfaces
{
    public interface IBookService
    {
        public Task<BookDto[]> GetAsync();

        public Task<long> CreateAsync(BookWithoutIdDto book);

        public Task AppendAsync(long bookId, int count);
    }
}

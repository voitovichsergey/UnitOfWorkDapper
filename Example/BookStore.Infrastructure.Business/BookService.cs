using AutoMapper;
using BookStore.Domain.Core;
using BookStore.Domain.Interfaces;
using BookStore.Services.Interfaces;
using BookStore.Services.Interfaces.Dto;
using EntityFramework.UnitOfWork.Interfaces;

namespace BookStore.Infrastructure.Business
{
    public sealed class BookService(IMapper mapper, IUnitOfWork unitOfWork) : IBookService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task AppendAsync(long bookId, int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);

            using (var uow = _unitOfWork.StartTransaction())
            {
                await uow.Repository<IBookRepository>().AppendAsync(bookId, count);
                await uow.CommitAsync();
            }
        }

        public async Task<long> CreateAsync(BookWithoutIdDto book)
        {
            var mappedBook = _mapper.Map<Book>(book);

            using (var uow = _unitOfWork.StartTransaction())
            {
                var id = await uow.Repository<IBookRepository>().CreateAsync(mappedBook);
                await uow.CommitAsync();
                return id;
            }
        }

        public async Task<BookDto[]> GetAsync()
        {
            using (var uow = _unitOfWork.Selector())
            {
                var books = await uow.Repository<IBookRepository>().GetAsync();
                return _mapper.Map<BookDto[]>(books);
            }
        }
    }
}

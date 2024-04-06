using BookStore.Services.Interfaces;
using BookStore.Services.Interfaces.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class BookController
    {
        private readonly IBookService _bookService;
        private readonly ISaleOrderService _saleOrderService;

        public BookController(IBookService bookService,
            ISaleOrderService saleOrderService)
        {
            _bookService = bookService;
            _saleOrderService = saleOrderService;
        }

        [HttpPost]
        [Route("Get")]
        public async Task<BookDto[]> Get()
        {
            return await _bookService.GetAsync();
        }

        [HttpPost]
        [Route("Append")]
        public async Task Append(long bookId, int count)
        {
            await _bookService.AppendAsync(bookId, count);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<long> Add(BookWithoutIdDto book)
        {
            return await _bookService.CreateAsync(book);
        }

        [HttpPost]
        [Route("Sale")]
        public async Task Sale(long bookId)
        {
            await _saleOrderService.Sale(bookId);
        }
    }
}

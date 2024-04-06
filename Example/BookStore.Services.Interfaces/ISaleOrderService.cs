namespace BookStore.Services.Interfaces
{
    public interface ISaleOrderService
    {
        public Task Sale(long bookId);
    }
}

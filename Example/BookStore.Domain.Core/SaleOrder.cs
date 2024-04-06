namespace BookStore.Domain.Core
{
    public sealed class SaleOrder
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public long BookId { get; set; }
    }
}

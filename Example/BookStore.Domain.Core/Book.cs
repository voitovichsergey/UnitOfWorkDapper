namespace BookStore.Domain.Core
{
    public sealed class Book
    {
        public long Id { get; set; }

        public string? Title { get; set; }

        public string? Author { get; set; }

        public long Count { get; set; }
    }
}

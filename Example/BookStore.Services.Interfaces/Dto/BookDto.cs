namespace BookStore.Services.Interfaces.Dto
{
    public sealed class BookDto : BookWithoutIdDto
    {
        public long Id { get; set; }

        public int Count { get; set; }
    }
}

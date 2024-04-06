using BookStore.Domain.Core;
using BookStore.Domain.Interfaces;
using System.Data;

namespace BookStore.Infrastructure.Data
{
    public sealed class BookRepository(IDbConnection connection) : IBookRepository
    {
        private readonly BookStoreConnection _connection = (BookStoreConnection)connection;

        public async Task<Book> GetAsync(long bookId)
        {
            var result = await GetBooksAsync(bookId);
            return result.First();
        }

        public async Task AppendAsync(long bookId, int count)
        {
            using var command = _connection.CreateCommand();
            command.CommandText = $"update books set count = count + {count} where id = {bookId}";
            var result = await command.ExecuteNonQueryAsync();
            if (result == 0) throw new NullReferenceException("Book not found");
        }

        public async Task<long> CreateAsync(Book book)
        {
            var id = await GetMaxIdAsync();
            using var command = _connection.CreateCommand();
            command.CommandText =
                $"insert into books (id, title, author, count) values ({id}, {Quote(book.Title)}, {Quote(book.Author)}, 0)";
            var result = await command.ExecuteNonQueryAsync();
            return id;
        }

        public async Task<Book[]> GetAsync()
        {
            return await GetBooksAsync();
        }

        private async Task<Book[]> GetBooksAsync(long? bookId = null)
        {
            var result = new List<Book>();
            using var command = _connection.CreateCommand();
            command.CommandText = $"select * from books {(bookId == null ? string.Empty : $"where id = {bookId}")}";
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new Book
                {
                    Id = (long)reader[nameof(Book.Id)],
                    Title = reader[nameof(Book.Title)].ToString(),
                    Author = reader[nameof(Book.Author)].ToString(),
                    Count = (long)reader[nameof(Book.Count)],
                });
            }

            return [.. result];
        }

        private async Task<long> GetMaxIdAsync()
        {
            using var command = _connection.CreateCommand();
            command.CommandText = $"select coalesce(max(id), 0) + 1 as maxid from books";
            using var reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();
            return (long)reader["maxid"];
        }

        private static string Quote(string? value)
        {
            return $"\"{value?.Replace("\"", "\"\"")}\"";
        }
    }
}

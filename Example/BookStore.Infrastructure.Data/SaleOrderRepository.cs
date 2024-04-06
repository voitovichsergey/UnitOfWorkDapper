using BookStore.Domain.Interfaces;
using System.Data;

namespace BookStore.Infrastructure.Data
{
    public sealed class SaleOrderRepository(IDbConnection connection) : ISaleOrderRepository
    {
        private readonly BookStoreConnection _connection = (BookStoreConnection)connection;

        public async Task SellAsync(long bookId)
        {
            var id = await GetMaxIdAsync();
            using var command = _connection.CreateCommand();
            command.CommandText =
                $"insert into orders (id, date, bookid) values ({id}, date('now'), {bookId})";
            await command.ExecuteNonQueryAsync();
        }

        private async Task<long> GetMaxIdAsync()
        {
            using var command = _connection.CreateCommand();
            command.CommandText = $"select coalesce(max(id), 0) + 1 as maxid from orders";
            using var reader = await command.ExecuteReaderAsync();
            await reader.ReadAsync();
            return (long)reader["maxid"];
        }
    }
}

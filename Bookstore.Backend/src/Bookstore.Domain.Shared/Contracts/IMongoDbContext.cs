using MongoDB.Driver;

namespace Bookstore.Domain.Shared.Contracts
{
    public interface IMongoDbContext
    {
        IMongoCollection<Book> GetCollection<Book>(string name);
    }
}

using Bookstore.Domain.Aggregates.UserAggregate;
using MongoDB.Bson;
using Path = Bookstore.Domain.ValueObjects.Path;

namespace Bookstore.Domain.Shared.Contracts
{
    public interface IUserRepository : IMongoRepository<User>
    {
        Task AddAdminAsync(Admin admin);
        Task<Admin> GetAdminAsync(string login);
        Task UpdateNameAsync(ObjectId id, string name);
        Task UpdateImageAsync(ObjectId id, Path image);
        Task AddToFavourites(ObjectId id, string bookId);
        Task RemoveFromFavourites(ObjectId id, string bookId);
    }
}

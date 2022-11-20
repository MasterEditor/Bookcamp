using System.Xml.Linq;
using Bookstore.Domain.Aggregates.BookAggregate;
using Bookstore.Domain.Aggregates.UserAggregate;
using Bookstore.Domain.Shared.Contracts;
using Bookstore.Infrustructure.Repository.Base;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bookstore.Infrustructure.Repository
{
    public class UserRepository : MongoRepository<User>, IUserRepository
    {
        private readonly IMongoCollection<Admin> _adminCollection;

        public UserRepository(IMongoDbContext context) : base(context)
        {
            _adminCollection = _context.GetCollection<Admin>(GetCollectionName(typeof(Admin)));
        }

        public async Task UpdateImageAsync(ObjectId id, Domain.ValueObjects.Path image)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            var update = Builders<User>.Update.Set(x => x.Image, image);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateNameAsync(ObjectId id, string name)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            var update = Builders<User>.Update.Set(x => x.Name, name);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task AddToFavourites(ObjectId id, string bookId)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            var update = Builders<User>.Update.Push(x => x.Favourites, bookId);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task RemoveFromFavourites(ObjectId id, string bookId)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, id);
            var update = Builders<User>.Update.Pull(x => x.Favourites, bookId);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task AddAdminAsync(Admin admin)
        {
            await _adminCollection.InsertOneAsync(admin);
        }

        public async Task<Admin> GetAdminAsync(string login)
        {
            return await _adminCollection.Find(x => x.Login == login)
                .FirstOrDefaultAsync();
        }
    }
}

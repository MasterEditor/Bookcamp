using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bookstore.Domain.Aggregates.BookAggregate;
using Bookstore.Domain.Aggregates.ReadAggregate;
using Bookstore.Domain.Aggregates.UserAggregate;
using Bookstore.Domain.Shared.Contracts;
using Bookstore.Infrustructure.Repository.Base;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bookstore.Infrustructure.Repository
{
    public class ReadRepository : MongoRepository<Read>, IReadRepository
    {
        public ReadRepository(IMongoDbContext context) : base(context)
        {
            
        }

        public async Task AddBook(ObjectId id, string bookId)
        {
            var filter = Builders<Read>.Filter.Eq(x => x.Id, id);
            var update = Builders<Read>.Update.Push(x => x.Books, bookId);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteBook(ObjectId id, string bookId)
        {
            var filter = Builders<Read>.Filter.Eq(x => x.Id, id);
            var update = Builders<Read>.Update.Pull(x => x.Books, bookId);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateCoverAsync(ObjectId id, Domain.ValueObjects.Path cover)
        {
            var filter = Builders<Read>.Filter.Eq(x => x.Id, id);
            var update = Builders<Read>.Update.Set(x => x.Cover, cover);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task<List<Read>> GetAllByUserId(string userId)
        {
            return await _collection.Find(x => x.UserId == userId).ToListAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore.Domain.Aggregates.NewsAggregate;
using Bookstore.Domain.Aggregates.ReadAggregate;
using Bookstore.Domain.Shared.Contracts;
using Bookstore.Infrustructure.Repository.Base;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bookstore.Infrustructure.Repository
{
    public class NewsRepository : MongoRepository<News>, INewsRepository
    {
        public NewsRepository(IMongoDbContext context) : base(context)
        {

        }

        public async Task ConfirmNewsAsync(ObjectId id)
        {
            var filter = Builders<News>.Filter.Eq(x => x.Id, id);
            var update = Builders<News>.Update.Set(x => x.IsConfirmed, true);

            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateImagesAsync(ObjectId id, Domain.ValueObjects.Path[] images)
        {
            var filter = Builders<News>.Filter.Eq(x => x.Id, id);
            var update = Builders<News>.Update.Set(x => x.Images, images);

            await _collection.UpdateOneAsync(filter, update);
        }
    }
}

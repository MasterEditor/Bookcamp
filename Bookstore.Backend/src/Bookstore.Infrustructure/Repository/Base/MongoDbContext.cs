using Bookstore.Domain.Models;
using Bookstore.Domain.Shared.Contracts;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Bookstore.Infrustructure.Repository.Base
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _db;
        private readonly MongoClient _mongoClient;

        public MongoDbContext(IOptions<MongoDbSettings> configuration)
        {
            _mongoClient = new MongoClient(configuration.Value.ConnectionString);
            _db = _mongoClient.GetDatabase(configuration.Value.DatabaseName);
        }

        public IMongoCollection<TDocument> GetCollection<TDocument>(string name)
        {
            return _db.GetCollection<TDocument>(name);
        }
    }
}

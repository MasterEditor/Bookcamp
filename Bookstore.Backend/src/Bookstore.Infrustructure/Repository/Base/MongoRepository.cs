using System.Linq.Expressions;
using System.Xml.Linq;
using Bookstore.Domain.Aggregates.Base;
using Bookstore.Domain.Aggregates.UserAggregate;
using Bookstore.Domain.Attributes;
using Bookstore.Domain.Shared.Contracts;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bookstore.Infrustructure.Repository.Base
{
    public class MongoRepository<TDocument> : IMongoRepository<TDocument>
    where TDocument : Document, IAggregateRoot
    {
        protected readonly IMongoCollection<TDocument> _collection;
        protected readonly IMongoDbContext _context;

        public MongoRepository(IMongoDbContext context)
        {
            _context = context;
            _collection = _context.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .First()).CollectionName;
        }

        public virtual async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return await _collection.Find(filterExpression)
                .FirstOrDefaultAsync();
        }

        public virtual async Task<List<TDocument>> FilterByWithPagesAsync(
            int page,
            int pageSize,
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return await _collection.Find(filterExpression)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public virtual async Task<List<TDocument>> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, object>> sortExpression,
            int limit)
        {
            return await _collection.Find(filterExpression)
                .SortByDescending(sortExpression)
                .Limit(limit)
                .ToListAsync();
        }

        public virtual async Task<List<TDocument>> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression,
            int limit)
        {
            return await _collection.Find(filterExpression)
                .Limit(limit)
                .ToListAsync();
        }

        public virtual async Task<List<TDocument>> FilterBy(
            FilterDefinition<TDocument> filter,
            int limit)
        {
            return await _collection.Find(filter)
                .Limit(limit)
                .ToListAsync();
        }

        public virtual async Task<List<TDocument>> FilterByWithPagesAsync(
            int page,
            int pageSize,
            FilterDefinition<TDocument> filter)
        {
            return await _collection.Find(filter)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public virtual async Task<TDocument> FindByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);

            return await _collection.Find(filter)
                .SingleOrDefaultAsync();
        }

        public virtual async Task InsertOneAsync(TDocument document)
        {
            await _collection.InsertOneAsync(document);
        }

        public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            await _collection.FindOneAndDeleteAsync(filterExpression);
        }

        public async Task DeleteByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);

            await _collection.FindOneAndDeleteAsync(filter);
        }

        public async Task<List<TDocument>> GetAll()
        {
            return await _collection.Find(_ => true)
                .ToListAsync();
        }
    }
}

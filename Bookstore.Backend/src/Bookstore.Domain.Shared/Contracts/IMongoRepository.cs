using System.Linq.Expressions;
using Bookstore.Domain.Aggregates.Base;
using MongoDB.Driver;

namespace Bookstore.Domain.Shared.Contracts
{
    public interface IMongoRepository<TDocument> where TDocument : Document, IAggregateRoot
    {
        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task<List<TDocument>> GetAll();
        Task<List<TDocument>> FilterByWithPagesAsync(int page, int pageSize, Expression<Func<TDocument, bool>> filterExpression);
        Task<List<TDocument>> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, object>> sortExpression,
            int limit);
        Task<List<TDocument>> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression,
            int limit);
        Task<List<TDocument>> FilterByWithPagesAsync(int page, int pageSize, FilterDefinition<TDocument> filter);
        Task<TDocument> FindByIdAsync(string id);
        Task InsertOneAsync(TDocument document);
        Task InsertManyAsync(ICollection<TDocument> documents);
        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task DeleteByIdAsync(string id);
    }
}

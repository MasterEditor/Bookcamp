using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore.Domain.Aggregates.ReadAggregate;
using Bookstore.Domain.Aggregates.UserAggregate;
using MongoDB.Bson;

namespace Bookstore.Domain.Shared.Contracts
{
    public interface IReadRepository : IMongoRepository<Read>
    {
        Task UpdateCoverAsync(ObjectId id, ValueObjects.Path cover);
        Task<List<Read>> GetAllByUserId(string userId);
        Task AddBook(ObjectId id, string bookId);
        Task DeleteBook(ObjectId id, string bookId);
    }
}

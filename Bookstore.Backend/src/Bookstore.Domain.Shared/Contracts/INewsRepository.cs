using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bookstore.Domain.Aggregates.NewsAggregate;
using MongoDB.Bson;

namespace Bookstore.Domain.Shared.Contracts
{
    public interface INewsRepository : IMongoRepository<News>
    {
        Task ConfirmNewsAsync(ObjectId id);
        Task UpdateImagesAsync(ObjectId id, Domain.ValueObjects.Path[] images);
    }
}

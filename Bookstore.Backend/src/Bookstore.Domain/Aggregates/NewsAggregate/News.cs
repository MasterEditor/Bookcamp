using Bookstore.Domain.Aggregates.Base;
using Bookstore.Domain.Attributes;

namespace Bookstore.Domain.Aggregates.NewsAggregate
{
    [BsonCollection("news")]
    public sealed class News : Document, IAggregateRoot
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime? EventDate { get; private set; }
        public List<ValueObjects.Path?> Images { get; private set; }
    }
}

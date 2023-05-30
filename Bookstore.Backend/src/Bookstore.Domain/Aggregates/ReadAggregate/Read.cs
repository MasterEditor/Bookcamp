using Bookstore.Domain.Aggregates.Base;
using Bookstore.Domain.Attributes;

namespace Bookstore.Domain.Aggregates.ReadAggregate
{
    [BsonCollection("reads")]
    public sealed class Read : Document, IAggregateRoot
    {
        public string Name { get; private set; }
        public string UserId { get; private set; }
        public List<string> Books { get; private set; }
        public ValueObjects.Path? Cover { get; private set; }

        public Read(
            string name,
            string userId)
        {
            Name = name;
            UserId = userId;
            Books = new List<string>();
        }
    }
}

using Bookstore.Domain.Aggregates.Base;
using Bookstore.Domain.Attributes;

namespace Bookstore.Domain.Aggregates.UserAggregate
{
    [BsonCollection("reads")]
    public sealed class ToRead : Document
    {
        public string Name { get; private set; }
        public string UserId { get; private set; }
        public List<string> Books { get; private set; }
        public ValueObjects.Path? Cover { get; private set; }
    }
}

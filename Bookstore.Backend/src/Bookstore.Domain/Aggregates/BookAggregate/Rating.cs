using Bookstore.Domain.Aggregates.Base;
using Bookstore.Domain.Attributes;

namespace Bookstore.Domain.Aggregates.BookAggregate
{
    [BsonCollection("ratings")]
    public sealed class Rating : Document
    {
        public int Value { get; private set; }
        public string BookId { get; private set; }
        public string UserId { get; private set; }

        public Rating(int value, string bookId, string userId)
        {
            Value = value;
            BookId = bookId;
            UserId = userId;
        }
    }
}

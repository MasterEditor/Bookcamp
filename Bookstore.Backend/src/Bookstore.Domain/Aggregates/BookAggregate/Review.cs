using Bookstore.Domain.Aggregates.Base;
using Bookstore.Domain.Attributes;
using Bookstore.Domain.ValueObjects;

namespace Bookstore.Domain.Aggregates.BookAggregate
{
    [BsonCollection("reviews")]
    public sealed class Review : Document
    {
        public string Text { get; private set; }
        public string BookId { get; private set; }
        public UserReview User { get; private set; }
        public DateTime AddedAt { get; private set; }

        public Review(string text, string bookId, UserReview user)
        {
            Text = text;
            BookId = bookId;
            User = user;
            AddedAt = DateTime.UtcNow;
        }
    }
}

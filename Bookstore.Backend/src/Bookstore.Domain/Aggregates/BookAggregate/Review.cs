using Bookstore.Domain.Aggregates.Base;
using Bookstore.Domain.Attributes;
using Bookstore.Domain.ValueObjects;

namespace Bookstore.Domain.Aggregates.BookAggregate
{
    [BsonCollection("reviews")]
    public sealed class Review : Document
    {
        public string Title { get; private set; }
        public string Body { get; private set; }
        public string BookId { get; private set; }
        public UserReview User { get; private set; }
        public DateTime AddedAt { get; private set; }
        public int Likes { get; private set; }
        public int Dislikes { get; private set; }
        public List<Message> Messages { get; private set; } = new();

        public Review(
            string title,
            string text,
            string bookId,
            UserReview user
            )
        {
            Title = title;
            Body = text;
            BookId = bookId;
            User = user;
            AddedAt = DateTime.UtcNow;
            Likes = 0;
            Dislikes = 0;
        }
    }
}

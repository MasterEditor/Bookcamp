using Bookstore.Domain.Aggregates.Base;

namespace Bookstore.Domain.Aggregates.BookAggregate
{
    public sealed record Message : ValueObject
    {
        public string Body { get; init; }
        public string UserId { get; init; }
        public DateTime AddedAt { get; init; }

        public Message(
            string body,
            string userId
            )
        {
            Body = body;
            UserId = userId;
            AddedAt = DateTime.UtcNow;
        }
    }
}

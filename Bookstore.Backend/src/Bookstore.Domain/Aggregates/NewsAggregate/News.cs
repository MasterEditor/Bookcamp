using Bookstore.Domain.Aggregates.Base;
using Bookstore.Domain.Attributes;

namespace Bookstore.Domain.Aggregates.NewsAggregate
{
    [BsonCollection("news")]
    public sealed class News : Document, IAggregateRoot
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Body { get; private set; }
        public DateTime EventDate { get; private set; }
        public ValueObjects.Path[]? Images { get; private set; }
        public bool IsConfirmed { get; private set; }
        public string? UserId { get; private set; }

        public News(
            string title, string description, string body, DateTime eventDate, bool isConfirmed, string? userId)
        {
            Title = title;
            Description = description;
            Body = body;
            EventDate = eventDate;
            IsConfirmed = isConfirmed;
            UserId = userId;
        }
    }
}

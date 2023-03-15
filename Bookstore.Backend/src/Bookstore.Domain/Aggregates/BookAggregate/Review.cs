using Bookstore.Domain.Aggregates.Base;
using Bookstore.Domain.Attributes;
using Bookstore.Domain.Common.Enums;
using Bookstore.Domain.ValueObjects;

namespace Bookstore.Domain.Aggregates.BookAggregate
{
    [BsonCollection("reviews")]
    public sealed class Review : Document
    {
        public string Title { get; private set; }
        public string Body { get; private set; }
        public string BookId { get; private set; }
        public string UserId { get; private set; }
        public ReviewType ReviewType { get; private set; }
        public DateTime AddedAt { get; private set; }
        public List<string> Likes { get; private set; }
        public List<string> Dislikes { get; private set; }

        public Review(
            string title,
            string body,
            string bookId,
            string userId,
            ReviewType reviewType
            )
        {
            Title = title;
            Body = body;
            BookId = bookId;
            UserId = userId;
            ReviewType = reviewType;
            AddedAt = DateTime.UtcNow;
            Likes = new List<string>();
            Dislikes = new List<string>();
        }

        public void Like(string userId)
        {
            if (Likes.FirstOrDefault(x => x.Equals(userId, StringComparison.OrdinalIgnoreCase)) is not null)
            {
                return;
            }

            Likes.Add(userId);

            if (Dislikes.FirstOrDefault(x => x.Equals(userId, StringComparison.OrdinalIgnoreCase)) is not null)
            {
                Dislikes.Remove(userId);
            }
        }

        public void Dislike(string userId)
        {
            if (Dislikes.FirstOrDefault(x => x.Equals(userId, StringComparison.OrdinalIgnoreCase)) is not null)
            {
                return;
            }

            Dislikes.Add(userId);

            if (Likes.FirstOrDefault(x => x.Equals(userId, StringComparison.OrdinalIgnoreCase)) is not null)
            {
                Likes.Remove(userId);
            }
        }
    }
}

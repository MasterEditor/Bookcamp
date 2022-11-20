using Bookstore.Domain.Aggregates.Base;

namespace Bookstore.Domain.ValueObjects
{
    public sealed record UserReview : ValueObject
    {
        public string UserId { get; init; }
        public string UserName { get; init; }
        public string ImageUrl { get; init; }

        public UserReview(
            string userId,
            string userName,
            string imageUrl)
        {
            UserId = userId;
            UserName = userName;
            ImageUrl = imageUrl;
        }
    }
}

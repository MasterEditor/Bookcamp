using Bookstore.Domain.Common.Enums;

namespace Bookstore.API.Models.AddReview
{
    public sealed class AddReviewRequest
    {
        public ReviewType Type { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string BookId { get; set; }
    }
}

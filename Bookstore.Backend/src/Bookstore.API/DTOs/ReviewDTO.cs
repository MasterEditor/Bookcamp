using Bookstore.Domain.Common.Enums;
using LanguageExt;

namespace Bookstore.API.DTOs
{
    public sealed class ReviewDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public ReviewType Type { get; set; }
        public Arr<string> Likes { get; set; }
        public Arr<string> Dislikes { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public string AddedTime { get; set; }
    }
}

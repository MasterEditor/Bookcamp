using Bookstore.Domain.Common.Enums;
using LanguageExt;

namespace Bookstore.API.DTOs
{
    public sealed record ReviewDTO
    {
        public string Id { get; init; }
        public string Title { get; init; }
        public string Body { get; init; }
        public ReviewType Type { get; init; }
        public DateTime AddedAt { get; init; }
        public Arr<string> Likes { get; init; }
        public Arr<string> Dislikes { get; init; }
        public string UserName { get; init; }
        public string ImageUrl { get; init; }
        public string AddedTime { get; init; }
    }
}

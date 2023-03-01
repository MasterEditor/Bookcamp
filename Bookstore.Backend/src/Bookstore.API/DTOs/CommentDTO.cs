namespace Bookstore.API.DTOs
{
    public sealed record CommentDTO
    {
        public string Id { get; set; }
        public string Comment { get; init; }
        public string UserName { get; init; }
        public string ImageUrl { get; init; }
        public string AddedTime { get; init; }
    }
}

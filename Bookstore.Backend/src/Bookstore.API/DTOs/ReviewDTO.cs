namespace Bookstore.API.DTOs
{
    public sealed record ReviewDTO
    {
        public string Id { get; set; }
        public string Review { get; init; }
        public string UserName { get; init; }
        public string ImageUrl { get; init; }
        public string AddedTime { get; init; }
    }
}

namespace Bookstore.API.DTOs
{
    public sealed record NewsDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public string[] ImagePaths { get; init; }
    }
}

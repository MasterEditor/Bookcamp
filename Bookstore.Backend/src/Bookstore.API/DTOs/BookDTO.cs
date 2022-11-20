namespace Bookstore.API.DTOs
{
    public sealed record BookDTO
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Author { get; init; }
        public string Genre { get; init; }
        public decimal Price { get; init; }
        public float Rating { get; init; }
        public string About { get; init; }
        public string Language { get; init; }
        public int Pages { get; init; }
        public string PublishDate { get; init; }
        public string Cover { get; init; }
        public string[] FragmentPaths { get; init; }
    }
}

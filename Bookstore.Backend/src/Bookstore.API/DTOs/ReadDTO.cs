namespace Bookstore.API.DTOs
{
    public sealed record ReadDTO
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Cover { get; init; }
        public List<BookDTO> Books { get; init; }
    }
}

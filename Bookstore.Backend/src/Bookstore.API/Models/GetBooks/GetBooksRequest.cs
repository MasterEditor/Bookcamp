namespace Bookstore.API.Models.GetBooks
{
    public class GetBooksRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Keywords { get; set; }
        public string? Genre { get; set; }
    }
}

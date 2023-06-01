namespace Bookstore.API.Models.GetNews
{
    public class GetNewsRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Keywords { get; set; }
    }
}

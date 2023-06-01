namespace Bookstore.API.Models.DeleteBookFromRead
{
    public sealed class DeleteBookFromReadRequest
    {
        public string ReadId { get; set; }
        public string BookId { get; set; }
    }
}

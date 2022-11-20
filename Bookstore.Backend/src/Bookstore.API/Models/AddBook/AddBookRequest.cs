namespace Bookstore.API.Models.AddBook
{
    public sealed class AddBookRequest
    {
        public string BookId { get; set; }
        public IEnumerable<IFormFile> Fragments { get; set; }
    }
}

namespace Bookstore.API.Models.AddReadList
{
    public sealed class AddReadRequest
    {
        public string Name { get; set; }
        public IFormFile Cover { get; set; }
    }
}

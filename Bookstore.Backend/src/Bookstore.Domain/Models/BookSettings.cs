namespace Bookstore.Domain.Models
{
    public class BookSettings
    {
        public string[] AllowedBookFormats { get; set; }
        public string UploadPath { get; set; }
    }
}

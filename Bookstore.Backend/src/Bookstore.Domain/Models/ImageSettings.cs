namespace Bookstore.Domain.Models
{
    public sealed class ImageSettings
    {
        public int ImageMaxSizeInBytes { get; set; }
        public int BytesInMb { get; set; }
        public string[] AllowedImageFormats { get; set; }
        public string UploadPath { get; set; }
    }
}

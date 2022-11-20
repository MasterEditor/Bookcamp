namespace Bookstore.API.DTOs
{
    public record UploadFileDTO
    {
        public string Extension { get; init; }
        public string UniqueName { get; init; }
    }
}

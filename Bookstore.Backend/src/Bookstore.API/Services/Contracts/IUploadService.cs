namespace Bookstore.API.Services.Contracts
{
    public interface IUploadService
    {
        Task<string> UploadFile(IFormFile file, string path, string uniqueName);
    }
}

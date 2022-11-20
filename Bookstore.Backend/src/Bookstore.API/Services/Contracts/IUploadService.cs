using Bookstore.API.DTOs;

namespace Bookstore.API.Services.Contracts
{
    public interface IUploadService
    {
        Task<UploadFileDTO> UploadFile(IFormFile file, string path, string uniqueName);
    }
}

using Bookstore.API.Services.Contracts;

namespace Bookstore.API.Services
{
    public class UploadService : IUploadService
    {
        public async Task<string> UploadFile(IFormFile file, string path, string uniqueName)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            byte[] arr = memoryStream.ToArray();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var extension = Path.GetExtension(file.FileName);
            var uploadFilePath = Path.Combine(path, $"{uniqueName}{extension}");

            using var stream = File.Create(uploadFilePath);
            await stream.WriteAsync(arr.AsMemory(0, arr.Length));

            return extension;
        }
    }
}

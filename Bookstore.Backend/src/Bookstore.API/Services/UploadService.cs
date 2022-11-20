using System.IO.Compression;
using Bookstore.API.DTOs;
using Bookstore.API.Services.Contracts;

namespace Bookstore.API.Services
{
    public class UploadService : IUploadService
    {
        public async Task<UploadFileDTO> UploadFile(IFormFile file, string path, string uniqueName)
        {
            byte[] arr;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                arr = memoryStream.ToArray();
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var extension = Path.GetExtension(file.FileName);

            var fileName = uniqueName + "-" + extension[1..];

            string uploadFilePath = Path.Combine(path, $"{fileName}{extension}");

            using (var stream = File.Create(uploadFilePath))
            {
                await stream.WriteAsync(arr.AsMemory(0, arr.Length));
            }

            if (extension == ".fb2")
            {
                string newExtension = ".zip";
                string archiveFilePath = Path.Combine(path, $"{fileName}{newExtension}");

                using(var archive = ZipFile.Open(archiveFilePath, ZipArchiveMode.Create))
                {
                    archive.CreateEntryFromFile(uploadFilePath, Path.GetFileName(uploadFilePath));
                }

                File.Delete(uploadFilePath);
            }

            return new()
            {
                UniqueName = fileName,
                Extension = extension
            };
        }
    }
}

using System.Runtime.CompilerServices;
using Bookstore.API.DTOs;
using Bookstore.API.Models.AddBookToRead;
using Bookstore.API.Models.AddReadList;
using Bookstore.API.Models.GetImage;
using Bookstore.API.Services.Contracts;
using Bookstore.Domain.Aggregates.BookAggregate;
using Bookstore.Domain.Aggregates.ReadAggregate;
using Bookstore.Domain.Exceptions;
using Bookstore.Domain.Models;
using Bookstore.Domain.Shared.Contracts;
using Bookstore.Infrustructure.Repository;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Options;

namespace Bookstore.API.Services
{
    public sealed class ReadService : IReadService
    {
        private readonly IReadRepository _readRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ImageSettings _imageSettings;
        private readonly IUploadService _uploadService;

        public ReadService(
            IReadRepository readRepository,
            IBookRepository bookRepository,
            IOptions<ImageSettings> imageOptions,
            IUploadService uploadService)
        {
            _readRepository = readRepository;
            _bookRepository = bookRepository;
            _imageSettings = imageOptions.Value;
            _uploadService = uploadService;
        }

        public async Task<Result<string>> AddRead(AddReadRequest request, string userId, string serverUrl)
        {
            var exRead = await _readRepository.FindOneAsync(x => x.Name == request.Name && x.UserId == userId);

            if (exRead is not null)
            {
                return new Result<string>(new ReadAlreadyExistsException());
            }

            ValidateCover(request.Cover);

            Read read = new(request.Name, userId);

            await _readRepository.InsertOneAsync(read);

            string readId = read.Id.ToString();

            var uploadCoverResult = await _uploadService.UploadFile(request.Cover, _imageSettings.UploadPath, readId);

            Domain.ValueObjects.Path coverPath = new(
                uploadCoverResult.UniqueName,
                uploadCoverResult.Extension,
                request.Cover.ContentType,
                $"{serverUrl}/api/books/covers/{uploadCoverResult.UniqueName}");

            await _readRepository.UpdateCoverAsync(read.Id, coverPath);

            return read.Id.ToString();
        }

        public async Task<Result<Unit>> AddBook(AddBookToReadRequest request, string userId)
        {
            var read = await _readRepository.FindByIdAsync(request.ReadId);

            if (read is null || read.UserId != userId)
            {
                return new Result<Unit>(new ReadNotFoundException());
            }

            var book = await _bookRepository.FindByIdAsync(request.BookId);

            if (book is null)
            {
                return new Result<Unit>(new BookNotFoundException());
            }

            if (read.Books.Any(x => x == request.BookId))
            {
                return new Result<Unit>(new BookAlreadyExistsException());
            }

            await _readRepository.AddBook(read.Id, request.BookId);

            return Unit.Default;
        }

        public async Task<Result<Unit>> DeleteRead(string id)
        {
            await _readRepository.DeleteByIdAsync(id);

            return Unit.Default;
        }

        public async Task<Result<Arr<ReadDTO>>> GetUserReads(string userId)
        {
            var user = await _readRepository.FindByIdAsync(userId);

            if (user is null)
            {
                return new Result<Arr<ReadDTO>>(new UserNotFoundException());
            }

            var reads = await _readRepository.GetAllByUserId(userId);

            return reads.Select(x => new ReadDTO()
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Cover = x.Cover?.Url ?? ""
            }).ToArr();
        }

        public async Task<Result<GetFileResponse>> GetCover(string id)
        {
            var objectId = id.Split('-').First();

            var read = await _readRepository.FindByIdAsync(objectId);

            if (read is null)
            {
                return new Result<GetFileResponse>(new BookNotFoundException());
            }

            string coverPath = Path.Combine(_imageSettings.UploadPath, read.Cover.PathValue) + read.Cover.Extension;

            byte[] cover = await File.ReadAllBytesAsync(coverPath);

            return new GetFileResponse()
            {
                Data = cover,
                ContentType = read.Cover.ContentType
            };
        }

        private Result<string> ValidateCover(IFormFile file)
        {
            if (file is null)
            {
                return new Result<string>(new InvalidFileException("Uploading file is empty"));
            }

            if (file.Length > _imageSettings.ImageMaxSizeInBytes)
            {
                return new Result<string>(
                    new InvalidFileException($"The total file size should not exceed {_imageSettings.ImageMaxSizeInBytes / _imageSettings.BytesInMb} MB"));
            }

            if (!_imageSettings.AllowedImageFormats.Contains(file.ContentType))
            {
                return new Result<string>(new InvalidFileException($"Not supported image format"));
            }

            return string.Empty;
        }
    }
}

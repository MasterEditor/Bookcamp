using System.Linq.Expressions;
using Bookstore.API.DTOs;
using Bookstore.API.Services.Contracts;
using Bookstore.Domain.Aggregates.NewsAggregate;
using LanguageExt;
using LanguageExt.Common;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using Bookstore.Domain.Shared.Contracts;
using Bookstore.API.Models.AddNews;
using Bookstore.Domain.Exceptions;
using Bookstore.Domain.Models;
using Microsoft.Extensions.Options;
using Bookstore.Domain.Aggregates.ReadAggregate;
using Bookstore.API.Models.GetImage;
using Bookstore.Infrustructure.Repository;
using Bookstore.Domain.Aggregates.UserAggregate;

namespace Bookstore.API.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IUploadService _uploadService;
        private readonly ImageSettings _imageSettings;

        public NewsService(
            INewsRepository newsRepository,
            IUploadService uploadService,
            IOptions<ImageSettings> imageOptions)
        {
            _newsRepository = newsRepository;
            _uploadService = uploadService;
            _imageSettings = imageOptions.Value;
        }

        public async Task<Result<Arr<NewsDTO>>> GetNews(int page, int pageSize, string? keywords = null)
        {
            Expression<Func<News, bool>> expression = _ => true;
            FilterDefinition<News>? filter = null;
            FilterDefinition<News>? fullTextFilter = null;
            var builder = Builders<News>.Filter;

            if (keywords is not null)
            {
                filter = builder.Regex(nameof(News.Title), new BsonRegularExpression(new Regex(keywords, RegexOptions.IgnoreCase)));
                fullTextFilter = Builders<News>.Filter.Text(keywords);
            }

            var searchResult = await _newsRepository.FilterBy(filter ?? expression, 0);

            if (searchResult.Count is 0)
            {
                if (keywords is not null)
                {
                    filter = builder.Regex(nameof(News.Description), new BsonRegularExpression(new Regex(keywords, RegexOptions.IgnoreCase)));

                    searchResult = await _newsRepository.FilterBy(filter, 0);
                }
            }

            List<News>? fullTextResult = new();

            if (fullTextFilter is not null)
            {
                fullTextResult = await _newsRepository.FilterBy(fullTextFilter, 0);
            }

            var result = searchResult
                .Except(fullTextResult)
                .Filter(x => x.IsConfirmed)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return result.Select(x => new NewsDTO()
            {
                Id = x.Id.ToString(),
                Title = x.Title,
                Description = x.Description,
                Date = x.EventDate,
                UserId = x.UserId,
                ImagePaths = x.Images?.Select(y => y.Url).ToArray()
            }).ToArr();
        }

        public async Task<Result<Arr<NewsDTO>>> GetUnconfirmedNews(int page, int pageSize)
        {
            var searchResult = await _newsRepository.FilterBy(x => !x.IsConfirmed, 0);

            var result = searchResult
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return result.Select(x => new NewsDTO()
            {
                Id = x.Id.ToString(),
                Title = x.Title,
                Description = x.Description,
                Date = x.EventDate,
                UserId = x.UserId,
                ImagePaths = x.Images?.Select(y => y.Url).ToArray()
            }).ToArr();
        }

        public async Task<Result<Unit>> ConfirmNews(string id)
        {
            var news = await _newsRepository.FindByIdAsync(id);

            if (news is null)
            {
                return new Result<Unit>(new NewsNotFoundException());
            }

            await _newsRepository.ConfirmNewsAsync(news.Id);

            return Unit.Default;
        }

        public async Task<Result<string>> AddNews(AddNewsRequest request, string userId, bool isAdmin, string serverUrl)
        {
            var hasImages = request.Images is not null && request.Images.Any();

            if (hasImages)
            {
                foreach (var image in request.Images)
                {
                    ValidateImage(image);
                }
            }

            News news = new(
                request.Title,
                request.Description,
                request.Body,
                DateTime.Now,
                isAdmin,
                userId);

            await _newsRepository.InsertOneAsync(news);

            var newsId = news.Id.ToString();

            if (hasImages)
            {
                var imagePaths = new List<Domain.ValueObjects.Path>();
                var counter = 0;

                foreach (var image in request.Images)
                {
                    var uploadCoverResult = await _uploadService.UploadFile(image, _imageSettings.UploadPath, newsId + "-" + counter++);

                    imagePaths.Add(new(
                        uploadCoverResult.UniqueName,
                        uploadCoverResult.Extension,
                        image.ContentType,
                        $"{serverUrl}/api/news/images/{uploadCoverResult.UniqueName}"));
                }

                await _newsRepository.UpdateImagesAsync(news.Id, imagePaths.ToArray());
            }

            return news.Id.ToString();
        }

        public async Task<Result<Unit>> DeleteNews(string id, string userId, bool isAdmin)
        {
            var news = await _newsRepository.FindByIdAsync(id);

            if (!isAdmin)
            {
                if (news is null || news.UserId != userId)
                {
                    return new Result<Unit>(new NewsNotFoundException());
                }
            }

            await _newsRepository.DeleteByIdAsync(id);

            return Unit.Default;
        }

        public async Task<Result<GetFileResponse>> GetImage(string id)
        {
            var objectId = id.Split('-').First();

            var news = await _newsRepository.FindByIdAsync(objectId);

            if (news is null)
            {
                return new Result<GetFileResponse>(new NewsNotFoundException());
            }

            var index = Convert.ToInt32(id.Split('-').Skip(1).First());

            string coverPath = Path.Combine(_imageSettings.UploadPath, news.Images[index].PathValue) + news.Images[index].Extension;

            byte[] cover = await File.ReadAllBytesAsync(coverPath);

            return new GetFileResponse()
            {
                Data = cover,
                ContentType = news.Images[index].ContentType
            };
        }

        private Result<string> ValidateImage(IFormFile file)
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

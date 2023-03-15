using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AutoMapper;
using Bookstore.API.DTOs;
using Bookstore.API.Extensions;
using Bookstore.API.Models.AddReview;
using Bookstore.API.Models.GetImage;
using Bookstore.API.Services.Contracts;
using Bookstore.Domain.Aggregates.BookAggregate;
using Bookstore.Domain.Exceptions;
using Bookstore.Domain.Models;
using Bookstore.Domain.Shared.Contracts;
using Bookstore.Domain.ValueObjects;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Path = System.IO.Path;

namespace Bookstore.API.Services
{
    public sealed class BookService : IBookService
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUploadService _uploadService;
        private readonly BookSettings _bookSettings;
        private readonly ImageSettings _imageSettings;

        public BookService(
            IMapper mapper,
            IBookRepository bookRepository,
            IUserRepository userRepository,
            IUploadService uploadService,
            IOptions<BookSettings> bookOptions,
            IOptions<ImageSettings> imageOptions
            )
        {
            _mapper = mapper;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _uploadService = uploadService;
            _bookSettings = bookOptions.Value;
            _imageSettings = imageOptions.Value;
        }

        public async Task<Result<string>> AddBook(
            Google.Apis.Books.v1.Data.Volume volume,
            IEnumerable<IFormFile> fragments,
            IFormFile cover,
            string serverUrl)
        {
            DateTime compareDate = DateTime.UtcNow.AddMinutes(-1);

            var exBook = await _bookRepository.FindOneAsync(x => x.Name == volume.VolumeInfo.Title
                && x.Author == volume.VolumeInfo.Authors[0]
                && x.AddedAt > compareDate);

            if (exBook is not null)
            {
                return new Result<string>(new BookAlreadyExistsException());
            }

            var validateBooks = ValidateBooks(fragments);

            if (validateBooks.IsFaulted)
            {
                return validateBooks;
            }

            var validateCover = ValidateCover(cover);

            if (validateCover.IsFaulted)
            {
                return validateCover;
            }

            string? language = volume.VolumeInfo.Language.ToLanguage();

            if (language is null)
            {
                return new Result<string>(new FormatException());
            }

            Book book = new(
                volume.VolumeInfo.Title,
                (decimal?)volume.SaleInfo.ListPrice.Amount ?? 0,
                volume.VolumeInfo.Authors[0],
                (float?)volume.VolumeInfo.AverageRating ?? 0,
                volume.VolumeInfo.PublishedDate,
                volume.VolumeInfo.PageCount ?? 0,
                volume.VolumeInfo.Description,
                language,
                volume.VolumeInfo.Categories.First().Split(' ').First());

            await _bookRepository.InsertOneAsync(book);

            string bookId = book.Id.ToString();

            List<Domain.ValueObjects.Path> fragmentPaths = new();

            foreach (var fragment in fragments)
            {
                var uploadResult = await _uploadService.UploadFile(fragment, _bookSettings.UploadPath, bookId);

                fragmentPaths.Add(new(
                    uploadResult.UniqueName,
                    uploadResult.Extension,
                    "application/zip",
                    $"{serverUrl}/api/books/fragments/{uploadResult.UniqueName}/{uploadResult.Extension}"));
            }

            var uploadCoverResult = await _uploadService.UploadFile(cover, _imageSettings.UploadPath, bookId);

            Domain.ValueObjects.Path coverPath = new(
                uploadCoverResult.UniqueName,
                uploadCoverResult.Extension,
                cover.ContentType,
                $"{serverUrl}/api/books/covers/{uploadCoverResult.UniqueName}");

            await _bookRepository.UpdateFragmentsAsync(book.Id, fragmentPaths.ToArray());
            await _bookRepository.UpdateCoverAsync(book.Id, coverPath);

            return new Result<string>(bookId);
        }

        public async Task<Result<Unit>> AddRating(string userId, string bookId, int rate)
        {
            Rating rating = await _bookRepository.GetRating(userId, bookId);

            if (rating is null)
            {
                rating = new(rate, bookId, userId);

                await _bookRepository.AddRating(rating);
            }
            else
            {
                await _bookRepository.UpdateRating(userId, bookId, rate);
            }

            var rates = await _bookRepository.GetRateByAllRates(bookId);

            await _bookRepository.UpdateBookRate(bookId, rates);

            return Unit.Default;
        }

        public async Task<Result<Unit>> AddComment(
            string comment,
            string bookId,
            string userId)
        {
            var ex = await _bookRepository.GetComment(userId, bookId);

            if (ex is not null)
            {
                return new Result<Unit>(new CommentAlreadyExistsException());
            }

            var user = await _userRepository.FindByIdAsync(userId);

            if (user is null)
            {
                return new Result<Unit>(new UserNotFoundException());
            }

            UserReview userReview = new(
                userId,
                user.Name,
                user.Image is not null ? user.Image.Url : "");

            Comment newReview = new(comment, bookId, userReview);

            await _bookRepository.AddComment(newReview);

            return Unit.Default;
        }

        public async Task<Result<Unit>> AddReview(AddReviewRequest request, string userId)
        {
            var ex = await _bookRepository.GetReview(userId, request.BookId);

            if (ex is not null)
            {
                return new Result<Unit>(new CommentAlreadyExistsException());
            }

            var user = await _userRepository.FindByIdAsync(userId);

            if (user is null)
            {
                return new Result<Unit>(new UserNotFoundException());
            }

            Review review = new(
                request.Title,
                request.Body,
                request.BookId,
                userId,
                request.Type);

            await _bookRepository.AddReview(review);

            return Unit.Default;
        }

        public async Task<Result<Unit>> DeleteBook(string id)
        {
            await _bookRepository.DeleteByIdAsync(id);

            await _bookRepository.DeleteCommentsAndRatingsByBookId(id);

            return Unit.Default;
        }

        public async Task<Result<Unit>> DeleteComment(string id)
        {
            await _bookRepository.DeleteCommentAsync(id);

            return Unit.Default;
        }

        public async Task<Result<Arr<BookDTO>>> GetBooks(int page, int pageSize, string? keywords = null, string? genre = null)
        {
            Expression<Func<Book, bool>> expression = _ => true;
            FilterDefinition<Book>? filter = null;
            var builder = Builders<Book>.Filter;

            if (keywords is not null)
            {
                filter = builder.Regex(nameof(Book.Name), new BsonRegularExpression(new Regex(keywords, RegexOptions.IgnoreCase)));
            }

            if (genre is not null)
            {
                expression = x => string.Equals(genre, x.Genre);
            }

            var result = await _bookRepository.FilterByWithPagesAsync(page, pageSize, filter ?? expression);

            if (result.Count == 0)
            {
                if (keywords is not null)
                {
                    filter = builder.Regex(nameof(Book.Author), new BsonRegularExpression(new Regex(keywords, RegexOptions.IgnoreCase)));

                    result = await _bookRepository.FilterByWithPagesAsync(page, pageSize, filter);
                }
            }

            return result.Select(x => new BookDTO()
            {
                Id = x.Id.ToString(),
                Author = x.Author,
                Genre = x.Genre,
                Name = x.Name,
                Price = x.Price,
                Cover = x.Cover.Url
            }).ToArr();
        }

        public async Task<Result<Arr<BookDTO>>> GetBooks(string author, string id)
        {
            var objectId = ObjectId.Parse(id);

            var result = await _bookRepository.FilterBy(x => x.Author == author && x.Id != objectId, 4);

            return result.Select(x => new BookDTO()
            {
                Id = x.Id.ToString(),
                Author = x.Author,
                Genre = x.Genre,
                Name = x.Name,
                Price = x.Price,
                Cover = x.Cover.Url
            }).ToArr();
        }

        public async Task<Result<GetFileResponse>> GetCover(string id)
        {
            var objectId = id.Split('-').First();

            var book = await _bookRepository.FindByIdAsync(objectId);

            if (book is null)
            {
                return new Result<GetFileResponse>(new BookNotFoundException());
            }

            string coverPath = Path.Combine(_imageSettings.UploadPath, book.Cover.PathValue) + book.Cover.Extension;

            byte[] cover = await File.ReadAllBytesAsync(coverPath);

            return new GetFileResponse()
            {
                Data = cover,
                ContentType = book.Cover.ContentType
            };
        }

        public async Task<Result<GetFileResponse>> GetFragment(string id, string ext)
        {
            var objectId = id.Split('-').First();

            var book = await _bookRepository.FindByIdAsync(objectId);

            if (book is null)
            {
                return new Result<GetFileResponse>(new BookNotFoundException());
            }

            var fragment = book.FragmentPaths.First(x => x.Extension == ext);

            string extension = fragment.Extension;

            if (extension == ".fb2")
            {
                extension = ".zip";
            }

            string fragmentPath = Path.Combine(_bookSettings.UploadPath, fragment.PathValue) + extension;

            byte[] bytes = await File.ReadAllBytesAsync(fragmentPath);

            return new GetFileResponse()
            {
                Data = bytes,
                ContentType = fragment.ContentType,
                FileName = fragment.PathValue + extension
            };
        }

        public async Task<Result<Arr<string>>> GetGenres()
        {
            var books = await _bookRepository.GetAll();

            return books.Select(x => x.Genre)
                .Distinct()
                .ToArr();
        }

        public async Task<Result<Arr<BookDTO>>> GetNewBooks(int number)
        {
            var result = await _bookRepository.FilterBy(_ => true, x => x.AddedAt, number);

            return result.Select(x => new BookDTO()
            {
                Id = x.Id.ToString(),
                Author = x.Author,
                Genre = x.Genre,
                Name = x.Name,
                Price = x.Price,
                Cover = x.Cover.Url
            }).ToArr();
        }

        public async Task<Result<BookDTO>> GetOneBook(string id)
        {
            var result = await _bookRepository.FindByIdAsync(id);

            if (result is null)
            {
                return new Result<BookDTO>(new BookNotFoundException());
            }

            return new BookDTO
            {
                Id = result.Id.ToString(),
                Author = result.Author,
                Genre = result.Genre,
                Name = result.Name,
                Price = result.Price,
                Rating = result.Rating,
                About = result.About,
                Language = result.Language,
                Pages = result.Pages,
                PublishDate = result.PublishDate,
                Cover = result.Cover.Url,
                FragmentPaths = result.FragmentPaths.Select(x => x.Url).ToArray()
            };
        }

        public Result<long> GetPages(int pageSize)
        {
            long count = _bookRepository.GetAllDocumentsCount();

            long pages = 1;

            if (count > pageSize)
            {
                pages = count / pageSize;

                if (count % pageSize != 0)
                {
                    pages++;
                }
            }

            return pages;
        }

        public async Task<Result<int>> GetRating(string userId, string bookId)
        {
            var rating = await _bookRepository.GetRating(userId, bookId);

            if (rating is null)
            {
                return 0;
            }

            return rating.Value;
        }

        public async Task<Result<Arr<CommentDTO>>> GetComments(string bookId, int? amount)
        {
            var comments = await _bookRepository.GetAllCommentsByBook(bookId, amount);

            if (comments is null
                || comments.Count == 0)
            {
                return new Result<Arr<CommentDTO>>(new Arr<CommentDTO>());
            }

            return comments.Select(x => new CommentDTO()
            {
                Id = x.Id.ToString(),
                ImageUrl = x.User.ImageUrl,
                Comment = x.Text,
                UserName = x.User.UserName,
                AddedTime = GetDateDifference(x.AddedAt)
            }).ToArr();

        }

        public async Task<Result<Arr<ReviewDTO>>> GetReviews(string bookId, int? amount)
        {
            var reviews = await _bookRepository.GetAllReviewsByBook(bookId, amount);

            if (reviews is null
                || reviews.Count == 0)
            {
                return new Result<Arr<ReviewDTO>>(new Arr<ReviewDTO>());
            }

            var users = await _userRepository.GetAll();

            return reviews.Select(x => new ReviewDTO()
            {
                Id = x.Id.ToString(),
                Title = x.Title,
                Body = x.Body,
                Dislikes = x.Dislikes.ToArr(),
                Likes = x.Likes.ToArr(),
                AddedTime = GetDateDifference(x.AddedAt),
                Type = x.ReviewType,
                UserName = users.First(u => u.Id.ToString() == x.UserId).Name,
                ImageUrl = users.First(u => u.Id.ToString() == x.UserId).Image?.Url,
            }).ToArr();
        }

        public async Task<Result<Arr<BookDTO>>> GetTopRateBooks(int number)
        {
            var result = await _bookRepository.FilterBy(_ => true, x => x.Rating, number);

            return result.Select(x => new BookDTO()
            {
                Id = x.Id.ToString(),
                Author = x.Author,
                Genre = x.Genre,
                Name = x.Name,
                Price = x.Price,
                Cover = x.Cover.Url,
            }).ToArr();
        }

        public async Task<Result<Arr<BookDTO>>> GetUserFavourites(string userId)
        {
            var user = await _userRepository.FindByIdAsync(userId);

            if (user is null)
            {
                return new Result<Arr<BookDTO>>(new UserNotFoundException());
            }

            var books = await _bookRepository.GetAllBooksByIds(user.Favourites);

            return books.Select(x => new BookDTO()
            {
                Id = x.Id.ToString(),
                Author = x.Author,
                Genre = x.Genre,
                Name = x.Name,
                Price = x.Price,
                Cover = x.Cover.Url,
            }).ToArr();
        }

        public async Task<Result<bool>> IsUserAddedComment(string bookId, string userId)
        {
            var review = await _bookRepository.GetComment(userId, bookId);

            if (review is null)
            {
                return false;
            }

            return true;
        }

        public async Task<Result<bool>> IsUserAddedReview(string bookId, string userId)
        {
            var review = await _bookRepository.GetReview(userId, bookId);

            if (review is null)
            {
                return false;
            }

            return true;
        }

        public async Task<Result<string>> UpdateCover(IFormFile file, string bookId, string serverUrl)
        {
            var valid = ValidateCover(file);

            if (valid.IsFaulted)
            {
                return valid;
            }

            var book = await _bookRepository.FindByIdAsync(bookId);

            if (book is null)
            {
                return new Result<string>(new BookNotFoundException());
            }

            var uploadCoverResult = await _uploadService.UploadFile(file, _imageSettings.UploadPath, bookId);

            Domain.ValueObjects.Path coverPath = new(
                uploadCoverResult.UniqueName,
                uploadCoverResult.Extension,
                file.ContentType,
                $"{serverUrl}/api/books/covers/{uploadCoverResult.UniqueName}");

            await _bookRepository.UpdateCoverAsync(book.Id, coverPath);

            return uploadCoverResult.UniqueName;
        }

        public async Task<Result<string>> UpdateFragment(IFormFile file, string bookId, string serverUrl)
        {
            var valid = ValidateFragment(file);

            if (valid.IsFaulted)
            {
                return valid;
            }

            var book = await _bookRepository.FindByIdAsync(bookId);

            if (book is null)
            {
                return new Result<string>(new BookNotFoundException());
            }

            var uploadResult = await _uploadService.UploadFile(file, _bookSettings.UploadPath, bookId);

            Domain.ValueObjects.Path fragmentPath = new(
                uploadResult.UniqueName,
                uploadResult.Extension,
                "application/zip",
                $"{serverUrl}/api/books/fragments/{uploadResult.UniqueName}/{uploadResult.Extension}");

            await _bookRepository.UpdateFragmentAsync(book.Id, fragmentPath);

            return uploadResult.UniqueName;
        }

        public async Task<Result<Unit>> DeleteFragment(string extension, string bookId)
        {
            var book = await _bookRepository.FindByIdAsync(bookId);

            if (book is null)
            {
                return new Result<Unit>(new BookNotFoundException());
            }

            await _bookRepository.DeleteFragmentAsync(book.Id, extension);

            return Unit.Default;
        }

        public async Task<Result<Unit>> LikeReview(string reviewId, string userId)
        {
            var review = await _bookRepository.GetReview(ObjectId.Parse(reviewId));

            if (review is null)
            {
                return new Result<Unit>(new ReviewNotFoundException());
            }

            review.Like(userId);

            await _bookRepository.UpdateReviewLikes(review.Id, review.Likes);
            await _bookRepository.UpdateReviewDislikes(review.Id, review.Dislikes);

            return Unit.Default;
        }

        public async Task<Result<Unit>> DislikeReview(string reviewId, string userId)
        {
            var review = await _bookRepository.GetReview(ObjectId.Parse(reviewId));

            if (review is null)
            {
                return new Result<Unit>(new ReviewNotFoundException());
            }

            review.Dislike(userId);

            await _bookRepository.UpdateReviewDislikes(review.Id, review.Dislikes);
            await _bookRepository.UpdateReviewLikes(review.Id, review.Likes);

            return Unit.Default;
        }

        public async Task<Result<string>> AddFragment(IFormFile file, string bookId, string serverUrl)
        {
            var valid = ValidateFragment(file);

            if (valid.IsFaulted)
            {
                return valid;
            }

            var book = await _bookRepository.FindByIdAsync(bookId);

            if (book is null)
            {
                return new Result<string>(new BookNotFoundException());
            }

            var extension = Path.GetExtension(file.FileName);

            if (book.FragmentPaths.Any(x => x.Extension == extension))
            {
                return new Result<string>(new InvalidFileException("File with such extension already exists"));
            }

            var uploadResult = await _uploadService.UploadFile(file, _bookSettings.UploadPath, bookId);

            Domain.ValueObjects.Path fragmentPath = new(
                uploadResult.UniqueName,
                uploadResult.Extension,
                "application/zip",
                $"{serverUrl}/api/books/fragments/{uploadResult.UniqueName}/{uploadResult.Extension}");

            await _bookRepository.AddFragmentAsync(book.Id, fragmentPath);

            return uploadResult.UniqueName;
        }

        private string GetDateDifference(DateTime date)
        {
            TimeSpan subs = DateTime.UtcNow.Subtract(date);

            int years = subs.Days / 365;

            if (years >= 1)
            {
                if (years == 1)
                {
                    return "1 year ago";
                }

                if (years > 1)
                {
                    return $"{years} years ago";
                }
            }

            int months = subs.Days / 29;

            if (months >= 1)
            {
                if (months == 1)
                {
                    return "1 month ago";
                }

                if (months > 1)
                {
                    return $"{months} months ago";
                }
            }

            int weeks = subs.Days / 7;

            if (weeks >= 1)
            {
                if (weeks == 1)
                {
                    return "1 week ago";
                }

                if (weeks > 1)
                {
                    return $"{weeks} weeks ago";
                }
            }

            int days = subs.Days / 1;

            if (days >= 1)
            {
                if (days == 1)
                {
                    return "1 day ago";
                }

                if (days > 1)
                {
                    return $"{days} days ago";
                }
            }

            if (subs.Hours >= 1)
            {
                if (subs.Hours == 1)
                {
                    return "1 hour ago";
                }

                if (subs.Hours > 1)
                {
                    return $"{subs.Hours} hours ago";
                }
            }

            if (subs.Minutes >= 1)
            {
                if (subs.Minutes == 1)
                {
                    return "1 minute ago";
                }

                if (subs.Minutes > 1)
                {
                    return $"{subs.Minutes} minutes ago";
                }
            }

            return $"{subs.Seconds} seconds ago";
        }

        private Result<string> ValidateBooks(IEnumerable<IFormFile> files)
        {
            if (files is null
                || !files.Any())
            {
                return new Result<string>(new InvalidFileException("Uploading file array is empty"));
            }

            foreach (var file in files)
            {
                if (file is null)
                {
                    return new Result<string>(new InvalidFileException("Uploading file is empty"));
                }

                if (!_bookSettings.AllowedBookFormats.Contains(file.ContentType))
                {
                    return new Result<string>(new InvalidFileException($"Not supported book format"));
                }
            }

            return string.Empty;
        }

        private Result<string> ValidateFragment(IFormFile file)
        {
            if (file is null)
            {
                return new Result<string>(new InvalidFileException("Uploading file is empty"));
            }

            if (file is null)
            {
                return new Result<string>(new InvalidFileException("Uploading file is empty"));
            }

            if (!_bookSettings.AllowedBookFormats.Contains(file.ContentType))
            {
                return new Result<string>(new InvalidFileException($"Not supported book format"));
            }

            return string.Empty;
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

using Bookstore.API.DTOs;
using Bookstore.API.Models.AddBook;
using Bookstore.API.Models.GetImage;
using Bookstore.API.Models.UploadImage;
using Google.Apis.Books.v1.Data;
using LanguageExt;
using LanguageExt.Common;

namespace Bookstore.API.Services.Contracts
{
    public interface IBookService
    {
        Task<Result<BookDTO>> GetOneBook(string id);
        Task<Result<GetFileResponse>> GetFragment(string id, string ext);
        Task<Result<GetFileResponse>> GetCover(string id);
        Task<Result<Arr<string>>> GetGenres();
        Task<Result<Unit>> DeleteBook(string id);
        Task<Result<Arr<BookDTO>>> GetBooks(int page, int pageSize, string? keywords = null, string? genre = null);
        Task<Result<Arr<BookDTO>>> GetBooks(string author, string id);
        Task<Result<string>> AddBook(
            Volume volume,
            IEnumerable<IFormFile> fragments,
            IFormFile cover,
            string serverUrl);
        Task<Result<Unit>> AddReview(
            string review,
            string bookId,
            string userId);
        Task<Result<Arr<ReviewDTO>>> GetReviews(string bookId);
        Task<Result<Unit>> AddRating(string userId, string bookId, int rate);
        Task<Result<int>> GetRating(string userId, string bookId);
        Task<Result<Arr<BookDTO>>> GetUserFavourites(string userId);
        Task<Result<Arr<BookDTO>>> GetNewBooks(int number);
        Task<Result<Arr<BookDTO>>> GetTopRateBooks(int number);
        Task<Result<Unit>> DeleteReview(string id);
        Task<Result<bool>> IsUserAddedReview(string bookId, string userId);
        Result<long> GetPages(int pageSize);
        Task<Result<string>> UpdateCover(IFormFile file, string bookId, string serverUrl);
        Task<Result<string>> UpdateFragment(IFormFile file, string bookId, string serverUrl);
        Task<Result<string>> AddFragment(IFormFile file, string bookId, string serverUrl);
        Task<Result<Unit>> DeleteFragment(string extension, string bookId);
    }
}

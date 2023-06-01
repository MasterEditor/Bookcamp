using Bookstore.API.DTOs;
using LanguageExt.Common;
using LanguageExt;
using Bookstore.API.Models.AddNews;
using Bookstore.API.Models.GetImage;

namespace Bookstore.API.Services.Contracts
{
    public interface INewsService
    {
        Task<Result<Arr<NewsDTO>>> GetNews(int page, int pageSize, string? keywords);
        Task<Result<Arr<NewsDTO>>> GetUnconfirmedNews(int page, int pageSize);
        Task<Result<Unit>> ConfirmNews(string id);
        Task<Result<string>> AddNews(AddNewsRequest request, string userId, bool isAdmin, string serverUrl);
        Task<Result<Unit>> DeleteNews(string id, string userId, bool isAdmin);
        Task<Result<GetFileResponse>> GetImage(string id);
    }
}

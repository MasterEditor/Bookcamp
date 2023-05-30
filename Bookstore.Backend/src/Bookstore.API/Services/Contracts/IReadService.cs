using Bookstore.API.Models.AddBookToRead;
using Bookstore.API.Models.AddReadList;
using Bookstore.API.Models.GetImage;
using LanguageExt;
using LanguageExt.Common;

namespace Bookstore.API.Services.Contracts
{
    public interface IReadService
    {
        Task<Result<GetFileResponse>> GetCover(string id);
        Task<Result<string>> AddRead(
            AddReadRequest request,
            string userId,
            string serverUrl);
        Task<Result<Unit>> AddBook(AddBookToReadRequest request, string userId);
    }
}

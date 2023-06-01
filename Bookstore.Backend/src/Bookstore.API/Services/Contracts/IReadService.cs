using Bookstore.API.Models.AddBookToRead;
using Bookstore.API.Models.AddReadList;
using Bookstore.API.Models.DeleteBookFromRead;
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
        Task<Result<Unit>> DeleteRead(string id, string userId);
        Task<Result<Unit>> AddBook(AddBookToReadRequest request, string userId);
        Task<Result<Unit>> DeleteBook(DeleteBookFromReadRequest request, string userId);
    }
}

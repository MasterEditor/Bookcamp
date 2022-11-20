using Bookstore.API.Models.GetImage;
using Bookstore.API.Models.LoginUser;
using Bookstore.API.Models.SignupUser;
using Bookstore.API.Models.UserData;
using LanguageExt;
using LanguageExt.Common;

namespace Bookstore.API.Services.Contracts
{
    public interface IUserService
    {
        Task<Result<string>> SignUp(SignUpUserRequest request);
        Task<Result<string>> Login(LoginUserRequest request);
        Task<Result<Unit>> DeleteUser(string email);
        Task<Result<Unit>> ChangeName(string id, string newName);
        Task<Result<string>> UpdateImage(string id, IFormFile image);
        Task<Result<UserDataResponse>> GetUserData(string id, string serverUrl);
        Task<Result<GetFileResponse>> GetImage(string id);
        Task<Result<Unit>> AddToFavourite(string id, string bookId);
        Task<Result<Unit>> RemoveFromFavourite(string id, string bookId);
        Task<Result<Arr<UserDataResponse>>> GetAllUsers(string serverUrl);
    }
}

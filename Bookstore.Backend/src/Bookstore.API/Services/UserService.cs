using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using Bookstore.API.Models.LoginUser;
using Bookstore.API.Models.SignupUser;
using Bookstore.API.Services.Contracts;
using Bookstore.Domain.Aggregates.UserAggregate;
using Bookstore.Domain.Exceptions;
using Bookstore.Domain.Models;
using Bookstore.Domain.Shared.Contracts;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using Bookstore.API.Models.UserData;
using Bookstore.API.Models.GetImage;
using LanguageExt.Pipes;

namespace Bookstore.API.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUploadService _uploadService;
        private readonly TokenSettings _tokenSettings;
        private readonly ImageSettings _imageSettings;

        public UserService(
            IUserRepository userRepository,
            IBookRepository bookRepository,
            IUploadService uploadService,
            IOptions<TokenSettings> tokenOptions,
            IOptions<ImageSettings> imageOptions)
        {
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _uploadService = uploadService;
            _tokenSettings = tokenOptions.Value;
            _imageSettings = imageOptions.Value;
        }

        public async Task<Result<Unit>> AddToFavourite(string id, string bookId)
        {
            var user = await _userRepository.FindByIdAsync(id);

            if (user is null)
            {
                return new Result<Unit>(new UserNotFoundException());
            }

            await _userRepository.AddToFavourites(user.Id, bookId);

            return Unit.Default;
        }

        public async Task<Result<Unit>> DeleteUser(string email)
        {
            var user = await _userRepository.FindOneAsync(x => x.Email == email);

            await _userRepository.DeleteByIdAsync(user.Id.ToString());

            await _bookRepository.DeleteReviewsAndRatingsByUserId(user.Id.ToString());

            return Unit.Default;
        }

        public async Task<Result<Unit>> RemoveFromFavourite(string id, string bookId)
        {
            var user = await _userRepository.FindByIdAsync(id);

            if (user is null)
            {
                return new Result<Unit>(new UserNotFoundException());
            }

            await _userRepository.RemoveFromFavourites(user.Id, bookId);

            return Unit.Default;
        }

        public async Task<Result<Unit>> ChangeName(string id, string newName)
        {
            var user = await _userRepository.FindByIdAsync(id);

            if (user is null)
            {
                return new Result<Unit>(new UserNotFoundException());
            }

            user.ChangeName(newName);

            await _userRepository.UpdateNameAsync(user.Id, newName);

            return Unit.Default;
        }

        public async Task<Result<GetFileResponse>> GetImage(string id)
        {
            var user = await _userRepository.FindByIdAsync(id);

            if (user is null)
            {
                return new Result<GetFileResponse>(new UserNotFoundException());
            }

            string imagePath = Path.Combine(_imageSettings.UploadPath, user.Image.PathValue) + user.Image.Extensions[0];

            byte[] image = await File.ReadAllBytesAsync(imagePath);

            return new GetFileResponse()
            {
                Data = image,
                ContentType = user.Image.ContentTypes[0]
            };
        }

        public async Task<Result<Arr<UserDataResponse>>> GetAllUsers(string serverUrl)
        {
            var users = await _userRepository.GetAll();

            return users.Select(user => new UserDataResponse()
            {
                Birth = DateOnly.FromDateTime(user.Birthday),
                Gender = user.Gender,
                Name = user.Name,
                Email = user.Email,
                RegisterDate = user.RegistratedAt.ToString("MM/dd/yyyy hh:mm tt"),
                Favourites = user.Favourites.ToArray(),
                ImageUrl = user.Image is not null ? $"{serverUrl}/api/user/images/{user.Id}" : ""
            }).ToArr();
        }

        public async Task<Result<UserDataResponse>> GetUserData(string id, string serverUrl)
        {
            var user = await _userRepository.FindByIdAsync(id);

            if(user is null)
            {
                return new Result<UserDataResponse>(new UserNotFoundException());
            }

            var result = new UserDataResponse()
            {
                Birth = DateOnly.FromDateTime(user.Birthday),
                Gender = user.Gender,
                Name = user.Name,
                Email = user.Email,
                RegisterDate = user.RegistratedAt.ToString("MM/dd/yyyy hh:mm tt"),
                Favourites = user.Favourites.ToArray()
            };

            if(user.Image is not null )
            {
                result.ImageUrl = $"{serverUrl}/api/user/images/{user.Id}";
            }

            return new Result<UserDataResponse>(result);
        }

        public async Task<Result<string>> Login(LoginUserRequest request)
        {
            var user = await _userRepository.FindOneAsync(x => x.Email == request.Email);

            var admin = await _userRepository.GetAdminAsync("admin");

            if (user is not null && user.VerifyPassword(request.Password))
            {
                return GenerateJWTToken(user);
            }
            else if (admin is not null && admin.VerifyPassword(request.Password))
            {
                return GenerateJWTToken(admin);
            }

            return new Result<string>(new UserNotFoundException());
        }

        public async Task<Result<string>> SignUp(SignUpUserRequest request)
        {
            var exUser = await _userRepository.FindOneAsync(x => x.Email == request.Email);

            if (exUser is not null)
            {
                return new Result<string>(new UserAlreadyExistsException(request.Email));
            }

            User user = new(
                request.Email,
                request.Name,
                request.Birthday,
                request.Gender,
                request.Password);

            await _userRepository.InsertOneAsync(user);

            return user.Id.ToString();
        }

        public async Task<Result<string>> UpdateImage(string id, IFormFile image)
        {
            var valid = Validate(image);

            if (valid.IsFaulted)
            {
                return valid;
            }

            var user = await _userRepository.FindByIdAsync(id);

            if (user is null)
            {
                return new Result<string>(new UserNotFoundException());
            }

            user.DeletePreviousImage(_imageSettings.UploadPath);

            string uniqueName = user.Id.ToString();

            var extension = await _uploadService.UploadFile(image, _imageSettings.UploadPath, uniqueName);

            var path = new Domain.ValueObjects.Path(
                uniqueName, 
                new string[] { extension },
                new string[] {image.ContentType});

            await _userRepository.UpdateImageAsync(user.Id, path);

            return uniqueName;
        }

        private string GenerateJWTToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var singKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey));
            var tokenExpiration = DateTime.UtcNow.AddMinutes(_tokenSettings.JwtTokenLifeTime);

            Claim role = new(ClaimTypes.Role, "User");

            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Expiration, tokenExpiration.ToString()),
                role
            };

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Issuer = _tokenSettings.Issuer,
                Audience = _tokenSettings.Audience,
                Expires = tokenExpiration,
                TokenType = "JWT",
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(singKey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateJWTToken(Admin admin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var singKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey));
            var tokenExpiration = DateTime.UtcNow.AddMinutes(_tokenSettings.JwtTokenLifeTime);

            Claim role = new(ClaimTypes.Role, "Admin");

            Claim[] claims = new Claim[]
            {
                new Claim("Id", admin.Id.ToString()),
                new Claim(ClaimTypes.Expiration, tokenExpiration.ToString()),
                role
            };

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Issuer = _tokenSettings.Issuer,
                Audience = _tokenSettings.Audience,
                Expires = tokenExpiration,
                TokenType = "JWT",
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(singKey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private Result<string> Validate(IFormFile file)
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

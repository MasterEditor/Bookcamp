using Bookstore.API.Extensions;
using Bookstore.API.Models;
using Bookstore.API.Models.ChangeUserName;
using Bookstore.API.Models.Favourite;
using Bookstore.API.Models.LoginUser;
using Bookstore.API.Models.SignupUser;
using Bookstore.API.Models.UploadImage;
using Bookstore.API.Services.Contracts;
using Bookstore.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bookstore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    [Authorize]
    public sealed class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;
        private readonly AdminSettings _adminSettings;

        public UserController(
            IUserService userService,
            IAdminService adminService,
            IOptions<AdminSettings> adminOptions)
        {
            _userService = userService;
            _adminService = adminService;
            _adminSettings = adminOptions.Value;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignupUser([FromBody] SignUpUserRequest request)
        {
            var response = await _userService.SignUp(request);

            return response.ToOk();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request)
        {
            var response = await _userService.Login(request);

            return response.Match<IActionResult>(success =>
            {
                CookieOptions tokenCookieOptions = new()
                {
                    Path = "/",
                    Domain = "localhost",
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    Secure = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                };

                CookieOptions roleCookieOptions = new()
                {
                    Path = "/",
                    Domain = "localhost",
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    Secure = true,
                    HttpOnly = false,
                    SameSite = SameSiteMode.None,
                };

                Response.Cookies.Append("bc_token", success, tokenCookieOptions);

                if (request.Email == _adminSettings.Login)
                {
                    Response.Cookies.Append("bc_role", "Admin", roleCookieOptions);
                    return Ok(new Response<string>("Admin"));
                }
                else
                {
                    Response.Cookies.Append("bc_role", "User", roleCookieOptions);
                    return Ok(new Response<string>("User"));
                }

            }, ex =>
            {
                return BadRequest(new Response<string>(ex.Message));
            });
        }

        [HttpPut("change-name")]
        public async Task<IActionResult> ChangeUserName([FromBody] ChangeUserNameRequest request)
        {
            string id = HttpContext.GetUserId();

            var response = await _userService.ChangeName(id, request.Name);

            return response.ToOk();
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest request)
        {
            string id = HttpContext.GetUserId();

            var serverUrl = $"{Request.Scheme}://{Request.Host}";

            var response = await _userService.UpdateImage(id, request.Image, serverUrl);

            return response.ToOk();
        }

        [AllowAnonymous]
        [HttpGet("images/{id}")]
        public async Task<IActionResult> GetImage([FromRoute] string id)
        {
            var response = await _userService.GetImage(id);

            return response.Match<IActionResult>(success =>
            {
                return File(success.Data, success.ContentType);

            }, ex =>
            {
                return BadRequest(new Response<string>(ex.Message));
            });

        }

        [HttpGet("me")]
        public async Task<IActionResult> UserData()
        {
            string id = HttpContext.GetUserId();

            var response = await _userService.GetUserData(id);

            return response.ToOk();
        }

        [HttpPost("add-favourite")]
        public async Task<IActionResult> AddFavourite([FromBody] FavouriteRequest request)
        {
            string id = HttpContext.GetUserId();

            var response = await _userService.AddToFavourite(id, request.BookId);

            return response.ToOk();
        }

        [HttpPost("remove-favourite")]
        public async Task<IActionResult> RemoveFavourite([FromBody] FavouriteRequest request)
        {
            string id = HttpContext.GetUserId();

            var response = await _userService.RemoveFromFavourite(id, request.BookId);

            return response.ToOk();
        }

        [HttpPost("admin")]
        [AllowAnonymous]
        public async Task<IActionResult> AddAdmin()
        {
            await _adminService.AddAdmin();

            return Ok();
        }

        [HttpGet("users")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsers();

            return response.ToOk();
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{email}")]
        public async Task<IActionResult> GetAllUsers([FromRoute] string email)
        {
            var response = await _userService.DeleteUser(email);

            return response.ToOk();
        }
    }
}

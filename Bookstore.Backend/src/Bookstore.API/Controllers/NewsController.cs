using Bookstore.API.Extensions;
using Bookstore.API.Models;
using Bookstore.API.Models.AddNews;
using Bookstore.API.Models.GetNews;
using Bookstore.API.Services;
using Bookstore.API.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNewsPage([FromQuery] GetNewsRequest request)
        {
            var response = await _newsService.GetNews(
                request.Page,
                request.PageSize,
                request.Keywords);

            return response.ToOk();
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("unconfirmed")]
        public async Task<IActionResult> GetUnconfirmedNewsPage([FromQuery] GetNewsRequest request)
        {
            var response = await _newsService.GetUnconfirmedNews(
                request.Page,
                request.PageSize);

            return response.ToOk();
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("confirm/{id}")]
        public async Task<IActionResult> ConfirmNews([FromRoute] string id)
        {
            var response = await _newsService.ConfirmNews(id);

            return response.ToOk();
        }

        [Authorize(Policy = "User")]
        [HttpPost("news")]
        public async Task<IActionResult> AddNews([FromForm] AddNewsRequest request)
        {
            var userId = HttpContext.GetUserId();
            var isAdmin = HttpContext.IsAdmin();

            var serverUrl = $"{Request.Scheme}://{Request.Host}";

            var response = await _newsService.AddNews(request, userId, isAdmin, serverUrl);

            return response.ToOk();
        }

        [Authorize(Policy = "User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews([FromRoute] string id)
        {
            var userId = HttpContext.GetUserId();
            var isAdmin = HttpContext.IsAdmin();

            var response = await _newsService.DeleteNews(id, userId, isAdmin);

            return response.ToOk();
        }

        [HttpGet("images/{id}")]
        public async Task<IActionResult> GetImage([FromRoute] string id)
        {
            var response = await _newsService.GetImage(id);

            return response.Match<IActionResult>(success =>
            {
                return File(success.Data, success.ContentType);

            }, ex =>
            {
                return BadRequest(new Response<string>(ex.Message));
            });
        }
    }
}

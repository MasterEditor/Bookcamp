using Bookstore.API.Extensions;
using Bookstore.API.Models;
using Bookstore.API.Models.AddBookToRead;
using Bookstore.API.Models.AddReadList;
using Bookstore.API.Services;
using Bookstore.API.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class ReadsController : ControllerBase
    {
        private readonly IReadService _readService;

        public ReadsController(IReadService readService)
        {
            _readService = readService;
        }

        [HttpPost("read")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> AddRead([FromForm] AddReadRequest request)
        {
            string id = HttpContext.GetUserId();

            var serverUrl = $"{Request.Scheme}://{Request.Host}";

            var response = await _readService.AddRead(request, id, serverUrl);

            return response.ToOk();
        }

        [HttpPost("add-book")]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> AddBook([FromBody] AddBookToReadRequest request)
        {
            string id = HttpContext.GetUserId();

            var response = await _readService.AddBook(request, id);

            return response.ToOk();
        }

        [HttpGet("covers/{id}")]
        public async Task<IActionResult> GetCover([FromRoute] string id)
        {
            var response = await _readService.GetCover(id);

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

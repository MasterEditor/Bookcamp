using Bookstore.API.Models;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult ToOk<TResult>(this Result<TResult> result)
        {
            return result.Match<IActionResult>(success =>
            {
                return new OkObjectResult(new Response<TResult>(success));
            }, ex =>
            {
                return new BadRequestObjectResult(new Response<string>(ex.Message));
            });
        }
    }
}

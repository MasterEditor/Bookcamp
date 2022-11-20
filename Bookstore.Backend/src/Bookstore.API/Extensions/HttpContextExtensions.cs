using System.Security.Claims;

namespace Bookstore.API.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserEmail(this HttpContext httpContext)
        {
            return httpContext.User.Claims.Single(x => x.Type == ClaimTypes.Email).Value;
        }

        public static string GetUserId(this HttpContext httpContext)
        {
            return httpContext.User.Claims.Single(x => x.Type == "Id").Value;
        }
    }
}

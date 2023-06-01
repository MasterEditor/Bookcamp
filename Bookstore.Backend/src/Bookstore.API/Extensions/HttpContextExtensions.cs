using System.Security.Claims;
using Bookstore.Domain.Common.Contants;

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

        public static bool IsAdmin(this HttpContext httpContext)
        {
            return httpContext.User.Claims.Single(x => x.Type == ClaimTypes.Role).Value
                is RoleConstants.ADMIN;
        }
    }
}

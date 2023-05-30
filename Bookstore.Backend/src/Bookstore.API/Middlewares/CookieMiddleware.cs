using Bookstore.Domain.Common.Contants;

namespace Bookstore.API.Middlewares
{
    public class AuthCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies[CookieConstants.BC_TOKEN];

            if(!string.IsNullOrEmpty(token))
            {
                context.Request.Headers.TryAdd("Authorization", $"Bearer {token}");
            }

            await _next(context);
        }
    }

    public static class CookieMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthCookieMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthCookieMiddleware>();
        }
    }
}


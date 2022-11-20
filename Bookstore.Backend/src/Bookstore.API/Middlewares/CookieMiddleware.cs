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
            var token = context.Request.Cookies["bc_token"];
            var role = context.Request.Cookies["bc_role"];

            if(!string.IsNullOrEmpty(token) 
                && !string.IsNullOrEmpty(role))
            {
                context.Request.Headers.Add("Authorization", $"Bearer {token}");
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


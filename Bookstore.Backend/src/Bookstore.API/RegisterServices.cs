using Bookstore.API.Services;
using Bookstore.API.Services.Contracts;

namespace Bookstore.API
{
    public static class RegisterServices
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<IReadService, ReadService>();
            services.AddScoped<INewsService, NewsService>();
        }
    }
}

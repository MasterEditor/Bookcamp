using Bookstore.Domain.Shared.Contracts;
using Bookstore.Infrustructure.Repository;
using Bookstore.Infrustructure.Repository.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Bookstore.Infrustructure
{
    public static class DependencyInjection
    {
        public static void AddInfrustructure(this IServiceCollection services)
        {
            services.AddScoped<IMongoDbContext, MongoDbContext>();
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IReadRepository, ReadRepository>();
            services.AddScoped<INewsRepository, NewsRepository>();
        }
    }
}

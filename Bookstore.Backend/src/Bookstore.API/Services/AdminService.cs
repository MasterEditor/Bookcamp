using Bookstore.API.Services.Contracts;
using Bookstore.Domain.Aggregates.UserAggregate;
using Bookstore.Domain.Shared.Contracts;

namespace Bookstore.API.Services
{
    public class AdminService : IAdminService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public AdminService(
            IBookRepository bookRepository,
            IUserRepository userRepository
            )
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public async Task AddAdmin()
        {
            Admin admin = new("admin", "123456");

            var ex = await _userRepository.GetAdminAsync("admin");

            if(ex is not null)
            {
                return;
            }

            await _userRepository.AddAdminAsync(admin);
        }
    }
}

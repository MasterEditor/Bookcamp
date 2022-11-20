using Bookstore.API.Services.Contracts;
using Bookstore.Domain.Aggregates.UserAggregate;
using Bookstore.Domain.Models;
using Bookstore.Domain.Shared.Contracts;
using Microsoft.Extensions.Options;

namespace Bookstore.API.Services
{
    public class AdminService : IAdminService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly AdminSettings _adminSettings;

        public AdminService(
            IBookRepository bookRepository,
            IUserRepository userRepository,
            IOptions<AdminSettings> options
            )
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _adminSettings = options.Value;
        }

        public async Task AddAdmin()
        {
            Admin admin = new(_adminSettings.Login, _adminSettings.Password);

            var ex = await _userRepository.GetAdminAsync(_adminSettings.Login);

            if(ex is not null)
            {
                return;
            }

            await _userRepository.AddAdminAsync(admin);
        }
    }
}

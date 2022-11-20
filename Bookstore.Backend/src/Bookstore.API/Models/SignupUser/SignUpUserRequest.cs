using Bookstore.Domain.Common.Enums;

namespace Bookstore.API.Models.SignupUser
{
    public sealed class SignUpUserRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateOnly Birthday { get; set; }
        public Genders Gender { get; set; }
    }
}

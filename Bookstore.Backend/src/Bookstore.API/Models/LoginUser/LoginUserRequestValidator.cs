using Bookstore.API.Extensions;
using FluentValidation;

namespace Bookstore.API.Models.LoginUser
{
    public sealed class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}

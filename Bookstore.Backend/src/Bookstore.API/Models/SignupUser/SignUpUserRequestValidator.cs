using Bookstore.API.Extensions;
using FluentValidation;

namespace Bookstore.API.Models.SignupUser
{
    public sealed class SignUpUserRequestValidator : AbstractValidator<SignUpUserRequest>
    {
        public SignUpUserRequestValidator()
        {
            RuleFor(x => x.Email).Email()
                .NotEmpty();
            RuleFor(x => x.Password).Password()
                .NotEmpty();
            RuleFor(x => x.Name).Name()
                .NotEmpty();
            RuleFor(x => x.Gender).IsInEnum()
                .WithMessage("There is no such gender")
                .NotEmpty();
            RuleFor(x => x.Birthday)
                .NotEmpty();
        }
    }
}

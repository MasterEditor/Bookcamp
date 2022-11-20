using Bookstore.API.Extensions;
using FluentValidation;

namespace Bookstore.API.Models.ChangeUserName
{
    public sealed class ChangeUserNameRequestValidator : AbstractValidator<ChangeUserNameRequest>
    {
        public ChangeUserNameRequestValidator()
        {
            RuleFor(x => x.Name).Name()
                .NotEmpty();
        }
    }
}

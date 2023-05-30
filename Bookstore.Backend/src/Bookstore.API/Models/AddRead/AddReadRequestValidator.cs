using Bookstore.Domain.Common.Contants;
using FluentValidation;

namespace Bookstore.API.Models.AddReadList
{
    public class AddReadRequestValidator: AbstractValidator<AddReadRequest>
    {
        public AddReadRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
            RuleFor(x => x.Cover)
                .NotEmpty();
        }
    }
}

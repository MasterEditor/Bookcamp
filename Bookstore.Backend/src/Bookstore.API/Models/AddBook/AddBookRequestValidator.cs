using Bookstore.Domain.Common.Contants;
using FluentValidation;

namespace Bookstore.API.Models.AddBook
{
    public sealed class AddBookRequestValidator : AbstractValidator<AddBookRequest>
    {
        public AddBookRequestValidator()
        {
            RuleFor(x => x.BookId)
                .NotEmpty();
            RuleFor(x => x.Fragments)
                .NotEmpty();
        }
    }
}

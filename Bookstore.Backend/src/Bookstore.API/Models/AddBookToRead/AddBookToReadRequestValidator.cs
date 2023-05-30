using Bookstore.API.Models.AddComment;
using FluentValidation;

namespace Bookstore.API.Models.AddBookToRead
{
    public class AddBookToReadRequestValidator : AbstractValidator<AddBookToReadRequest>
    {
        public AddBookToReadRequestValidator()
        {
            RuleFor(x => x.BookId).NotEmpty();
            RuleFor(x => x.ReadId).NotEmpty();
        }
    }
}

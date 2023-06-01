using FluentValidation;

namespace Bookstore.API.Models.DeleteBookFromRead
{
    public class DeleteBookFromReadRequestValidator : AbstractValidator<DeleteBookFromReadRequest>
    {
        public DeleteBookFromReadRequestValidator()
        {
            RuleFor(x => x.BookId).NotEmpty();
            RuleFor(x => x.ReadId).NotEmpty();
        }
    }
}

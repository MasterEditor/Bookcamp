using FluentValidation;

namespace Bookstore.API.Models.AddComment
{
    public class AddCommentRequestValidator : AbstractValidator<AddCommentRequest>
    {
        public AddCommentRequestValidator()
        {
            RuleFor(x => x.Comment)
                .MinimumLength(10)
                .MaximumLength(256)
                .NotEmpty(); 
            RuleFor(x => x.BookId).NotEmpty();
        }
    }
}

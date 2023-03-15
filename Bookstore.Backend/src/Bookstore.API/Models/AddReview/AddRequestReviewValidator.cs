using FluentValidation;

namespace Bookstore.API.Models.AddReview
{
    public sealed class AddRequestReviewValidator : AbstractValidator<AddReviewRequest>
    {
        public AddRequestReviewValidator()
        {
            RuleFor(x => x.Type).IsInEnum()
                .WithMessage("There is no such review type")
                .NotEmpty();
            RuleFor(x => x.Title)
                .MinimumLength(2)
                .MaximumLength(32)
                .NotEmpty();
            RuleFor(x => x.Body)
                .MinimumLength(256)
                .MaximumLength(2048)
                .NotEmpty();
            RuleFor(x => x.BookId)
                .NotEmpty();
        }
    }
}

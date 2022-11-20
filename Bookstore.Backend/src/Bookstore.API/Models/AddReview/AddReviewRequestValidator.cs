using FluentValidation;

namespace Bookstore.API.Models.AddReview
{
    public class AddReviewRequestValidator : AbstractValidator<AddReviewRequest>
    {
        public AddReviewRequestValidator()
        {
            RuleFor(x => x.Review)
                .MinimumLength(10)
                .MaximumLength(256)
                .NotEmpty(); 
            RuleFor(x => x.BookId).NotEmpty();
        }
    }
}

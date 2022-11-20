using FluentValidation;

namespace Bookstore.API.Models.Rate
{
    public class AddRatingRequestValidator : AbstractValidator<AddRatingRequest>
    {
        public AddRatingRequestValidator()
        {
            RuleFor(x => x.BookId).NotEmpty();
            RuleFor(x => x.Rate)
                .GreaterThan(0)
                .LessThanOrEqualTo(5)
                .NotEmpty();
        }
    }
}

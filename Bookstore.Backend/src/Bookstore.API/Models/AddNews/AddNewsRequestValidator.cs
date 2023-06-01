using FluentValidation;

namespace Bookstore.API.Models.AddNews
{
    public class AddNewsRequestValidator : AbstractValidator<AddNewsRequest>
    {
        public AddNewsRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty();
            RuleFor(x => x.Description)
                .NotEmpty();
            RuleFor(x => x.Body)
                .NotEmpty();
        }
    }
}

using FluentValidation;

namespace Bookstore.API.Models.GetNews
{
    public class GetNewsRequestValidator : AbstractValidator<GetNewsRequest>
    {
        public GetNewsRequestValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .NotEmpty();
            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .NotEmpty();
        }
    }
}

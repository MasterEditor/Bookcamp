using FluentValidation;

namespace Bookstore.API.Models.GetBooks
{
    public class GetBooksRequestValidator : AbstractValidator<GetBooksRequest>
    {
        public GetBooksRequestValidator()
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

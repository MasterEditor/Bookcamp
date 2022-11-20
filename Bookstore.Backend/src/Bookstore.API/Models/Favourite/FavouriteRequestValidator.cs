using FluentValidation;

namespace Bookstore.API.Models.Favourite
{
    public class FavouriteRequestValidator : AbstractValidator<FavouriteRequest>
    {
        public FavouriteRequestValidator()
        {
            RuleFor(x => x.BookId).NotEmpty(); 
        }
    }
}

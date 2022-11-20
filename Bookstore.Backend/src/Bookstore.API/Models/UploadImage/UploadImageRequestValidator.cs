using FluentValidation;

namespace Bookstore.API.Models.UploadImage
{
    public sealed class UploadImageRequestValidator : AbstractValidator<UploadImageRequest>
    {
        public UploadImageRequestValidator()
        {
            RuleFor(x => x.Image).NotEmpty();
        }
    }
}

using FluentValidation;

namespace Bookstore.API.Models.UploadImage
{
    public sealed class UploadFileRequestValidator : AbstractValidator<UploadFileRequest>
    {
        public UploadFileRequestValidator()
        {
            RuleFor(x => x.File).NotEmpty();
        }
    }
}

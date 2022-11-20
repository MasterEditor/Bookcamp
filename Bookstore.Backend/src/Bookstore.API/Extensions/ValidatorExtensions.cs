using Bookstore.Domain.Extensions;
using FluentValidation;

namespace Bookstore.API.Extensions
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(x => !string.IsNullOrEmpty(x) && x.IsEmailValid())
                .WithMessage("Email is not valid");
        }

        public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(x => !string.IsNullOrEmpty(x) && x.IsPasswordValid())
                .WithMessage("Password must be no shorter than eight characters, at least one uppercase letter, one lowercase letter and one number");
        }

        public static IRuleBuilderOptions<T, string> Name<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(x => !string.IsNullOrEmpty(x) && x.IsNameValid())
                .WithMessage("Name must be no shorter than 2 and no longer than 32 characters. Only eng alphabet, digits and symbol of '-'");
        }
    }
}

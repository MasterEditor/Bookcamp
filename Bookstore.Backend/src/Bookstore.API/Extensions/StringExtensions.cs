using Bookstore.Domain.Common.Contants;

namespace Bookstore.API.Extensions
{
    public static class StringExtensions
    {
        public static string? ToLanguage(this string value)
        {
            var fullName = Languages.AllLangs
                .Where(x => x.Abbr.Equals(value, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.FullName)
                .SingleOrDefault();

            return fullName;
        }
    }
}

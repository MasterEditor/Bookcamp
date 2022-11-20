using System.Text.RegularExpressions;

namespace Bookstore.Domain.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmailValid(this string str)
        {
            Regex regex = new(@"^.{1,64}[@]{1}[a-zA-Z0-9-]{2,}([.]{1}[a-zA-Z0-9-]+)+$");

            return Validate(str, regex.ToString());
        }

        public static bool IsNameValid(this string str)
        {
            Regex regex = new(@"^[a-zA-Z0-9-]{2,32}$");

            return Validate(str, regex.ToString());
        }

        public static bool IsPasswordValid(this string str)
        {
            Regex regex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,32}$");

            return Validate(str, regex.ToString());
        }

        private static bool Validate(string str, string regex)
        {
            try
            {
                return Regex.IsMatch(str,
                    regex,
                    RegexOptions.IgnoreCase,
                    TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}

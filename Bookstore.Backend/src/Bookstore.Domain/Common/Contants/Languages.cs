namespace Bookstore.Domain.Common.Contants
{
    public struct Languages
    {
        public static readonly LanguagePair RU = new("ru", "Russian");
        public static readonly LanguagePair EN = new("en", "English");

        public static readonly IReadOnlyCollection<LanguagePair> AllLangs = new List<LanguagePair>
        {
            RU,
            EN
        }.AsReadOnly();
    }

    public struct LanguagePair
    {
        public string Abbr { get; set; }
        public string FullName { get; set; }

        public LanguagePair(string abbr, string fullName)
        {
            Abbr = abbr;
            FullName = fullName;
        }
    }
}

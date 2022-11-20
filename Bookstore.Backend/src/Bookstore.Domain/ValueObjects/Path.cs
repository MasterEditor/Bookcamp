using Bookstore.Domain.Aggregates.Base;

namespace Bookstore.Domain.ValueObjects
{
    public sealed record Path : ValueObject
    {
        public string PathValue { get; private set; }
        public string[] Extensions { get; private set; }
        public string[] ContentTypes { get; private set; }

        public Path(
            string pathValue,
            string[] extensions,
            string[] contentTypes 
            )
        {
            PathValue = pathValue;
            Extensions = extensions;
            ContentTypes = contentTypes;
        }
    }
}

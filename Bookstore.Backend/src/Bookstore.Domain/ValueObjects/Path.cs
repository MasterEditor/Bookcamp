using Bookstore.Domain.Aggregates.Base;

namespace Bookstore.Domain.ValueObjects
{
    public sealed record Path : ValueObject
    {
        public string PathValue { get; private set; }
        public string Extension { get; private set; }
        public string ContentType { get; private set; }

        public Path(
            string pathValue,
            string extension,
            string contentType
            )
        {
            PathValue = pathValue;
            Extension = extension;
            ContentType = contentType;
        }
    }
}

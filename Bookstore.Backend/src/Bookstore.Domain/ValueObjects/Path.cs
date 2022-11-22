using Bookstore.Domain.Aggregates.Base;

namespace Bookstore.Domain.ValueObjects
{
    public sealed record Path : ValueObject
    {
        public string PathValue { get; private set; }
        public string Extension { get; private set; }
        public string ContentType { get; private set; }
        public string Url { get; private set; }

        public Path(
            string pathValue,
            string extension,
            string contentType,
            string url
            )
        {
            PathValue = pathValue;
            Extension = extension;
            ContentType = contentType;
            Url = url;
        }
    }
}

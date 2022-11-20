using Bookstore.Domain.Aggregates.Base;
using Bookstore.Domain.Attributes;
using static System.Net.Mime.MediaTypeNames;

namespace Bookstore.Domain.Aggregates.BookAggregate
{
    [BsonCollection("books")]
    public sealed class Book : Document, IAggregateRoot
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Author { get; private set; }
        public float Rating { get; private set; }
        public string PublishDate { get; private set; }
        public int Pages { get; private set; }
        public string About { get; private set; }
        public string Language { get; private set; }
        public string Genre { get; private set; }
        public ValueObjects.Path FragmentPath { get; private set; }
        public string Cover { get; private set; }
        public DateTime AddedAt { get; private set; }

        public Book(
            string name,
            decimal price,
            string author,
            float rating,
            string publishDate,
            int pages,
            string about,
            string language,
            string genre,
            string cover
            )
        {
            Name = name;
            Price = price;
            Author = author;
            Rating = rating;
            PublishDate = publishDate;
            Pages = pages;
            About = about;
            Language = language;
            Genre = genre;
            AddedAt = DateTime.UtcNow;
            Cover = cover;
        }
    }
}

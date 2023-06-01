using Bookstore.Domain.Aggregates.Base;

namespace Bookstore.API.Models.AddNews
{
    public sealed class AddNewsRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public IEnumerable<IFormFile>? Images { get; set; }
    }
}

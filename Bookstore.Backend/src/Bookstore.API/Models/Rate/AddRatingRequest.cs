namespace Bookstore.API.Models.Rate
{
    public class AddRatingRequest
    {
        public string BookId { get; set; }
        public int Rate { get; set; }
    }
}

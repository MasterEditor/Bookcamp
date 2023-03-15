namespace Bookstore.Domain.Exceptions
{
    public class ReviewNotFoundException : Exception
    {
        public ReviewNotFoundException() : base($"Review does not exist")
        {

        }
    }
}

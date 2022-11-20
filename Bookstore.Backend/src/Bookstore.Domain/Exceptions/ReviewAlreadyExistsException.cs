namespace Bookstore.Domain.Exceptions
{
    public class ReviewAlreadyExistsException : Exception
    {
        public ReviewAlreadyExistsException() : base($"Review already exists")
        {

        }
    }
}

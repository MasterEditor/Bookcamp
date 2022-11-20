namespace Bookstore.Domain.Exceptions
{
    public class BookAlreadyExistsException : Exception
    {
        public BookAlreadyExistsException() : base($"Such book already exists")
        {

        }
    }
}

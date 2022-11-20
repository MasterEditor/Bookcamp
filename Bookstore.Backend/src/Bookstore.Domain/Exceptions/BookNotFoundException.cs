namespace Bookstore.Domain.Exceptions
{
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException() : base($"Book does not exist")
        {

        }
    }
}

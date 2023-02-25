namespace Bookstore.Domain.Exceptions
{
    public class CommentAlreadyExistsException : Exception
    {
        public CommentAlreadyExistsException() : base($"Review already exists")
        {

        }
    }
}

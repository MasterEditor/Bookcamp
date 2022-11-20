namespace Bookstore.API.Models
{
    public class Response<T>
    {
        public T Body { get; set; }

        public Response(T body)
        {
            Body = body;
        }
    }
}

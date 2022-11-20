using Bookstore.Domain.Aggregates.Base;
using Bookstore.Domain.Attributes;

namespace Bookstore.Domain.Aggregates.UserAggregate
{
    [BsonCollection("admins")]
    public class Admin : Document
    {
        public string Login { get; private set; }
        public string PasswordHashed { get; private set; }
        public string Salt { get; private set; }
        public DateTime CreatedDate { get; private set; }

        public Admin(string login, string password)
        {
            Login = login;
            Salt = Crypt.GetSalt();
            PasswordHashed = Crypt.HashString(password, Salt);
            CreatedDate = DateTime.Now;
        }

        public bool VerifyPassword(string password)
        {
            return Crypt.Verify(password, PasswordHashed, Salt);
        }
    }
}

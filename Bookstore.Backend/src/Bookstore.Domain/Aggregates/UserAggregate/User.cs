using Bookstore.Domain.Aggregates.Base;
using Bookstore.Domain.Attributes;
using Bookstore.Domain.Common.Enums;
using LanguageExt.Pipes;

namespace Bookstore.Domain.Aggregates.UserAggregate
{
    [BsonCollection("users")]
    public sealed class User : Document, IAggregateRoot
    {
        public string Email { get; private set; }
        public string Name { get; private set; }
        public DateTime Birthday { get; private set; }
        public Genders Gender { get; private set; }
        public ValueObjects.Path? Image { get; private set; }
        public string PasswordHashed { get; private set; }
        public string Salt { get; private set; }
        public DateTime RegistratedAt { get; private set; }
        public List<string> Favourites { get; private set; } = new();

        public User(
            string email,
            string name,
            DateOnly birthday,
            Genders gender,
            string password
            )
        {
            Email = email;
            Name = name;
            Birthday = new DateTime(birthday.Year, birthday.Month, birthday.Day);
            Gender = gender;
            Salt = Crypt.GetSalt();
            PasswordHashed = Crypt.HashString(password, Salt);
            RegistratedAt = DateTime.UtcNow;
        }

        public bool VerifyPassword(string password)
        {
            return Crypt.Verify(password, PasswordHashed, Salt);
        }

        public void ChangeName(string newName)
        {
            Name = newName;
        }

        public void DeletePreviousImage(string uploadPath)
        {
            if (Image is null)
            {
                return;
            }

            var previousImagePath = Path.Combine(uploadPath, Image.PathValue + Image.Extension);

            if (File.Exists(previousImagePath))
            {
                File.Delete(previousImagePath);
            }
        }
    }
}

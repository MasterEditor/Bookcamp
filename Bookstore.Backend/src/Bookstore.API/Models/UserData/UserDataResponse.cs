using Bookstore.Domain.Common.Enums;

namespace Bookstore.API.Models.UserData
{
    public sealed class UserDataResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public DateOnly Birth { get; set; }
        public Genders Gender { get; set; }
        public string RegisterDate { get; set; }
        public string[] Favourites { get; set; }
    }
}

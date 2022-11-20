namespace Bookstore.Domain.Models
{
    public sealed class TokenSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int JwtTokenLifeTime { get; set; }
    }
}

namespace API.Infrastructure.Jwt
{
    public class JwtResult
    {
        public string Token { get; set; }

        public long Expiration { get; set; }
    }
}

namespace API.Infrastructure.Jwt
{
    public class JwtResult
    {
        public string EncodedToken { get; set; }
        public long Expiration { get; set; }
    }
}

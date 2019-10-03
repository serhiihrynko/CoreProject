namespace API.Infrastructure.Jwt
{
    public class JwtOptions
    {
        public string SecurityKey { get; set; }

        public int Expires { get; set; } // minutes

        //public string Issuer { get; set; }

        //public string Audience { get; set; }
    }
}

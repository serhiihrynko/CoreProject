namespace API.Infrastructure.Jwt
{
    public interface IJwtFactory
    {
        JwtResult GenerateToken(string userId);
    }
}

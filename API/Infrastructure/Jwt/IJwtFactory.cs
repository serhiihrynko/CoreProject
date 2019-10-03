using Domain.Entities;

namespace API.Infrastructure.Jwt
{
    public interface IJwtFactory
    {
        JwtResult GetJwt(User user);
    }
}

using Domain.Entities;
using System.Collections.Generic;

namespace API.Infrastructure.Jwt
{
    public interface IJwtFactory
    {
        JwtResult GetJwt(User user, IEnumerable<string> userRoles);
    }
}

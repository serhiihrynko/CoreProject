using Domain.Entities;
using System.Collections.Generic;

namespace API.Infrastructure.Jwt
{
    public interface IJwtFactory
    {
        JwtResult GenerateToken(string userId);
    }
}

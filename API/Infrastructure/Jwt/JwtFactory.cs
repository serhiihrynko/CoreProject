using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace API.Infrastructure.Jwt
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtOptions _tokenOptions;

        public JwtFactory(IOptions<JwtOptions> tokenOptions)
        {
            _tokenOptions = tokenOptions.Value;
        }


        public JwtResult GetJwt(User user)
        {
            var token = GenerateToken(user);

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            var expiration = new DateTimeOffset(token.ValidTo, TimeSpan.Zero).ToUnixTimeSeconds(); // timestamp

            var result = new JwtResult()
            {
                EncodedToken = encodedToken,
                Expiration = expiration
            };

            return result;
        }


        private  JwtSecurityToken GenerateToken(User user)
        {
            var claims = GenerateClaims(user);

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));

            var token = new JwtSecurityToken(
                //issuer: "",
                //audience: "",
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_tokenOptions.Expires)),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        private IEnumerable<Claim> GenerateClaims(User user)
        {
            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
        }
    }
}

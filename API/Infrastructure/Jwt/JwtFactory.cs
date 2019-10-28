using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Infrastructure.Jwt
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtOptions _tokenOptions;

        public JwtFactory(IOptions<JwtOptions> tokenOptions)
        {
            _tokenOptions = tokenOptions.Value;
        }


        public JwtResult GenerateToken(string userId)
        {
            var token = Create(userId);

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            var expiration = new DateTimeOffset(token.ValidTo, TimeSpan.Zero).ToUnixTimeSeconds(); // timestamp

            var jwtResult = new JwtResult()
            {
                Token = encodedToken,
                Expiration = expiration
            };

            return jwtResult;
        }


        private JwtSecurityToken Create(string userId)
        {
            var claims = new Claim[] {new Claim(JwtRegisteredClaimNamesCustom.UserId, userId)};

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
    }
}
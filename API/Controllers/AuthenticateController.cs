using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using API.Infrastructure.Jwt;

namespace API.Controllers
{
    [Route("[controller]")]
    public class AuthenticateController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenManagement _tokenManagement;

        public AuthenticateController(
            UserManager<User> userManager,
            IOptions<TokenManagement> tokenManagementOptions)
        {
            _userManager = userManager;
            _tokenManagement = tokenManagementOptions.Value;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if ((user == null) || (!await _userManager.CheckPasswordAsync(user, model.Password)))
            {
                return BadRequest("Invalid email or password.");
            }

            var authClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.SecurityKey));

            var token = new JwtSecurityToken(
                //issuer: "",
                //audience: "",
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_tokenManagement.Expires)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = new DateTimeOffset(token.ValidTo, TimeSpan.Zero).ToUnixTimeSeconds() // timestamp
            });
        }
    }
}

using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API.Models;
using API.Infrastructure.Jwt;

namespace API.Controllers
{
    [Route("authenticate")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtFactory _jwtFactory;

        public AuthenticateController(
            UserManager<User> userManager,
            IJwtFactory jwtFactory)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
        }


        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(JwtResult), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid email or password.");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if ((user == null) || (!await _userManager.CheckPasswordAsync(user, model.Password)))
            {
                return BadRequest("Invalid email or password.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = _jwtFactory.GetJwt(user, userRoles);

            return Ok(result);
        }
    }
}

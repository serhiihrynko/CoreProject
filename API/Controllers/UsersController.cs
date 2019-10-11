using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API.Extensions;
using API.Models;
using API.Infrastructure.Identity;

namespace API.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody]CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    Email = model.Email,
                    UserName = model.UserName
                };

                var createUser = await _userManager.CreateAsync(user, model.Password);

                if (createUser.Succeeded)
                {
                    var setRole = await _userManager.AddToRoleAsync(user, RoleConstants.User);

                    if (setRole.Succeeded)
                    {
                        return Ok(new
                        {
                            UserId = user.Id,
                            Role = RoleConstants.User
                        });
                    }

                    ModelState.AddErrorsToModelState(setRole.Errors);
                }

                ModelState.AddErrorsToModelState(createUser.Errors);
            }

            return BadRequest(ModelState);
        }
    }
}

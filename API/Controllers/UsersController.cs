using API.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using API.Models.Users;

namespace API.Controllers
{
    [Route("Users")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody]CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User() {
                    Email = model.Email,
                    UserName = model.UserName
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok(new { UserId = user.Id });
                }

                ModelState.AddErrorsToModelState(result.Errors);
            }

            return BadRequest(ModelState);
        }
    }
}

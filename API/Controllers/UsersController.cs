using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API.Extensions;
using API.Models;
using API.Infrastructure.Identity;
using AutoMapper;

namespace API.Controllers
{
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UsersController(
            UserManager<User> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }


        [HttpPost]
        [ProducesResponseType(typeof(CreateUserResponse), 200)]
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
                        return Ok(_mapper.Map<CreateUserResponse>(user));
                    }

                    ModelState.AddErrorsToModelState(setRole.Errors);
                }

                ModelState.AddErrorsToModelState(createUser.Errors);
            }

            return BadRequest(ModelState);
        }
    }
}

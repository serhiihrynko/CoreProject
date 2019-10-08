using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API.Extensions;
using API.Models;
using AutoMapper;

namespace API.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class UsersController : Controller
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
        [AllowAnonymous]
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

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok(_mapper.Map<CreateUserResponse>(user));
                }

                ModelState.AddErrorsToModelState(result.Errors);
            }

            return BadRequest(ModelState);
        }
    }
}

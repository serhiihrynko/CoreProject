using API.Infrastructure;
using API.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    public class StatusController : Controller
    {
        private readonly string _message;

        public StatusController(UptimeService uptimeService)
        {
            _message = $"API is running... (Uptime: {uptimeService.Uptime}, Started at {uptimeService.TimeStarted})";
        }


        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<string> GetStatus()
        {
            return Ok(_message);
        }

        [HttpGet("authorized")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public ActionResult<string> GetStatusAuthorized()
        {
            return Ok(_message);
        }

        [HttpGet("authorized/admin")]
        [Authorize(Roles = RoleConstants.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public ActionResult<string> GetStatusAuthorizeAdmin()
        {
            return Ok(_message);
        }
    }
}

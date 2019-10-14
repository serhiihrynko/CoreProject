using API.Infrastructure;
using API.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("status")]
    public class StatusController : ControllerBase
    {
        private readonly string _message;

        public StatusController(UptimeService uptimeService)
        {
            _message = $"API is running... (Uptime: {uptimeService.Uptime}, Started at {uptimeService.TimeStarted})";
        }


        [HttpGet]
        [ProducesResponseType(200)]
        public string GetStatus() => _message;

        [HttpGet("authorized")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public string GetStatusAuthorized() => _message;

        [HttpGet("authorized/admin")]
        [Authorize(Roles = RoleConstants.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public string GetStatusAuthorizeAdmin() => _message;
    }
}

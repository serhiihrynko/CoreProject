using API.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("")]
    [Authorize]
    public class StatusController : Controller
    {
        private readonly string _message;
       
        public StatusController(UptimeService uptimeService)
        {
            _message = $"API is running... (Uptime: {uptimeService.Uptime}, Started at {uptimeService.TimeStarted})";
        }


        [HttpGet("authorized")]
        public ActionResult<string> GetStatusAuthorized()
        {
            return Ok(_message);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<string> GetStatus()
        {
            return Ok(_message);
        }
    }
}

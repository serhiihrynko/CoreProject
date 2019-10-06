using API.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class StatusController : Controller
    {
        private readonly string _message;
       
        public StatusController(UptimeService uptimeService)
        {
            _message = $"API is running... (Uptime: {uptimeService.Uptime}, Started at {uptimeService.TimeStarted})";
        }


        [HttpGet("authorized")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public ActionResult<string> GetStatusAuthorized()
        {
            return Ok(_message);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public ActionResult<string> GetStatus()
        {
            return Ok(_message);
        }
    }
}

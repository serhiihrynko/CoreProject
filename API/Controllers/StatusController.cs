using API.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("")]
    public class StatusController : Controller
    {
        private readonly string message;
       
        public StatusController(UptimeService uptimeService)
        {
            message = $"API is running... (Uptime: {uptimeService.Uptime}, Started at {uptimeService.TimeStarted})";
        }


        [HttpGet]
        public ActionResult<string> GetStatus()
        {
            return Ok(message);
        }
    }
}

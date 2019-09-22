using API.Infrastructure;
using API.Infrastructure.Email;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("")]
    public class StatusController : Controller
    {
        private readonly string message;

        private readonly IEmailService _emailService;

        public StatusController(UptimeService uptimeService, IEmailService emailService)
        {
            message = $"API is running... (Uptime: {uptimeService.Uptime}, Started at {uptimeService.TimeStarted})";
            _emailService = emailService;
        }


        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok(message);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using TrackerApp.API.Model;
using TrackerApp.API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrackerApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        private readonly NotificationService _service;

        public NotificationController(NotificationService service)
        {
            _service = service;
        }

        [HttpGet("/all")]
        public async Task<IActionResult> GetAll()
        {
            var notifications = await _service.GetNotifications();
            return Ok(notifications);
        }

        [HttpPost("/notification")]
        public async Task<IActionResult> Create(Notification notification)
        {
            await _service.AddNotification(notification);
            return Ok();
        }


    }
}

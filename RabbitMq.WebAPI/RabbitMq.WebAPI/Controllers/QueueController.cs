using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMq.Services.Abstract;

namespace RabbitMq.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QueueController : ControllerBase
    {
        private readonly IQueueService _service;

        public QueueController(IQueueService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotifications(int userId) =>
            Ok(await _service.GetAllNotifications(userId));
    }
}

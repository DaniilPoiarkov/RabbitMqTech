using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMq.Broker.Services;
using RabbitMq.Common.DTOs.NotificationsDto;
using RabbitMq.Common.Entities.Notifications;

namespace RabbitMq.WebAPI.Controllers.Notifications
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController<TNotification, TService, TDto> : ControllerBase
        where TNotification : Notification
        where TDto : NotificationDto
        where TService : NotificationService<TNotification, TDto>
    {
        private readonly TService _service;
        public NotificationController(TService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications(int userId)
        {
            return Ok(await _service.GetNotifications(userId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] TDto notification)
        {
            await _service.CreateAndSendNotification(notification);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            await _service.DeleteNotification(id);
            return NoContent();
        }
    }
}

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
    public class PrivateNotificationController : 
        NotificationController<PrivateNotification, PrivateNotificationService, PrivateNotificationDto>
    {
        public PrivateNotificationController(PrivateNotificationService service) : base(service) { }
    }
}

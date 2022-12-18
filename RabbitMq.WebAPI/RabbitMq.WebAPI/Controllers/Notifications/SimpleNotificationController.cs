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
    public class SimpleNotificationController :
        NotificationController<SimpleNotification, SimpleNotificationService, SimpleNotificationDto>
    {
        public SimpleNotificationController(SimpleNotificationService service) : base(service) { }
    }
}


namespace RabbitMq.Common.DTOs.NotificationsDto
{
    public class SimpleNotificationDto : NotificationDto
    {
        public override string ToString() => $"Id: {Id}, Created: {CreatedAt}, Content: {Content}";
    }
}

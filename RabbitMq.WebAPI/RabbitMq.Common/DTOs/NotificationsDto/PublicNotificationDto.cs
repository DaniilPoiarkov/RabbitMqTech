
namespace RabbitMq.Common.DTOs.NotificationsDto
{
    public class PublicNotificationDto : NotificationDto
    {
        public override string ToString() => $"Id: {Id}, Created: {CreatedAt}, Content: {Content}";
    }
}

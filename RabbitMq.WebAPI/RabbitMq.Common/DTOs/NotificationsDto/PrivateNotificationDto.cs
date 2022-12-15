
namespace RabbitMq.Common.DTOs.NotificationsDto
{
    public class PrivateNotificationDto : NotificationDto
    {
        public int SenderId { get; set; }
        public UserDto? Sender { get; set; }
        public override string ToString() => $"Id: {Id}, Created: {CreatedAt}, Content: {Content}, SenderId: {SenderId}";
    }
}

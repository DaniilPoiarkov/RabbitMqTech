
namespace RabbitMq.Common.Entities.Notifications
{
    public class PrivateNotification : Notification
    {
        public int SenderId { get; set; }
        public override string ToString() => $"Id: {Id}, Created: {CreatedAt}, Content: {Content}, SenderId: {SenderId}";
    }
}

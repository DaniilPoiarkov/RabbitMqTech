
namespace RabbitMq.Common.Entities.Notifications
{
    public class PublicNotification : Notification
    {
        public override string ToString() => $"Id: {Id}, Created: {CreatedAt}, Content: {Content}";
    }
}

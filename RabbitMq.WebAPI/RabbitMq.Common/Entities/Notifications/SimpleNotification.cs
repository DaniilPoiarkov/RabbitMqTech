
namespace RabbitMq.Common.Entities.Notifications
{
    public class SimpleNotification : Notification
    {
        public override string ToString() => $"Id: {Id}, Created: {CreatedAt}, Content: {Content}";
    }
}

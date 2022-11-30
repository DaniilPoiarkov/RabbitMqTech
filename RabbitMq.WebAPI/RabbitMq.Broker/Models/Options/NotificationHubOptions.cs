
namespace RabbitMq.Broker.Models.Options
{
    public class NotificationHubOptions
    {
        public string PrivateNotificationsQueue { get; set; } = null!;
        public string SimpleNotificationsQueue { get; set; } = null!;
        public string ExchangeName { get; set; } = "Notifications";
    }
}

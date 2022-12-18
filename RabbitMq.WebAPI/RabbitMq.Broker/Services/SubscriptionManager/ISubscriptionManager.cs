using RabbitMq.Common.Entities.Notifications;
using RabbitMQ.Client.Events;

namespace RabbitMq.Broker.Hubs.SubscriptionHelper
{
    public interface ISubscriptionManager
    {
        Task<bool> GetValue<TNotification>(object? sender, BasicDeliverEventArgs args)
            where TNotification : Notification;
    }
}

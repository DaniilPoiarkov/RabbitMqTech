using Microsoft.AspNetCore.SignalR;
using RabbitMq.Broker.Hubs.SubscriptionHelper;
using RabbitMq.Broker.Interfaces;
using RabbitMq.Common.Entities.Notifications;

namespace RabbitMq.Broker.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IConsumerScope _consumerScopePrivate;
        private readonly IConsumerScope _consumerScopeSimple;
        private readonly ISubscriptionManager _subscriptionManager;
        public NotificationHub(
            IConsumerScope privateConsumer,
            IConsumerScope simpleConsumer,
            ISubscriptionManager subscriptionManager)
        {
            _subscriptionManager = subscriptionManager;
            _consumerScopePrivate = privateConsumer;
            _consumerScopeSimple = simpleConsumer;

            _consumerScopePrivate.Consumer.Recieved += async (s, args) =>
            {
                if (await _subscriptionManager.GetValue<PrivateNotification>(s, args))
                    _consumerScopePrivate.Consumer.SetAcknowledge(args.DeliveryTag, true);
                else
                    _consumerScopePrivate.Consumer.SetAcknowledge(args.DeliveryTag, false);
            };

            _consumerScopeSimple.Consumer.Recieved += async (s, args) =>
            {
                if (await _subscriptionManager.GetValue<SimpleNotification>(s, args))
                    _consumerScopeSimple.Consumer.SetAcknowledge(args.DeliveryTag, true);
                else
                    _consumerScopeSimple.Consumer.SetAcknowledge(args.DeliveryTag, false);
            };
        }

        public override Task OnConnectedAsync()
        {
            base.OnConnectedAsync();
            return Clients.Client(Context.ConnectionId).SendAsync("connected", Context.ConnectionId);
        }
    }
}

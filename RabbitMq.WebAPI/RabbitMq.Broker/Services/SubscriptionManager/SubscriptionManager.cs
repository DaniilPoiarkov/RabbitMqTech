using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMq.Common.Entities.Notifications;
using RabbitMQ.Client.Events;
using Serilog;
using Serilog.Events;
using System.Text;

namespace RabbitMq.Broker.Hubs.SubscriptionHelper
{
    public class SubscriptionManager : ISubscriptionManager
    {
        private readonly IHubContext<NotificationHub> _context;
        private readonly ILogger _logger;
        public SubscriptionManager(
            IHubContext<NotificationHub> context,
            ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> GetValue<TNotification>(object? sender, BasicDeliverEventArgs args)
            where TNotification : Notification
        {
            try
            {
                var json = Encoding.UTF8.GetString(args.Body.ToArray());
                var notification = JsonConvert.DeserializeObject<TNotification>(json);

                if (notification == null)
                    throw new Exception("Cannot deserialize notification");

                await _context.Clients.Client(notification.RecieverConnectionId)
                    .SendAsync(typeof(TNotification).Name, notification);

                return true;
            }
            catch (Exception ex)
            {
                if(_logger.IsEnabled(LogEventLevel.Warning))
                    _logger.Warning("Exception: {ex.Message}, SetAcknowledge to false", ex.Message);

                return false;
            }
        }
    }
}

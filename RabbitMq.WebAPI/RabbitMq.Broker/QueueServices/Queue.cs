using RabbitMq.Broker.Interfaces;
using RabbitMq.Broker.Models;
using RabbitMQ.Client;

namespace RabbitMq.Broker.QueueServices
{
    public class Queue : IQueue
    {
        private readonly IConnection _connection;
        private bool _disposed = false;
        public IModel Chanel { get; protected set; }
        public Queue(IConnectionFactory factory)
        {
            _connection = factory.CreateConnection();
            Chanel = _connection.CreateModel();
        }
        public Queue(IConnectionFactory factory, ScopeSettings settings)
            : this(factory)
        {
            DeclareExchange(settings.ExchangeName, settings.ExchangeType);

            if (settings.QueueName != null &&
                settings.RoutingKey != null)
                BindQueue(settings.ExchangeName, settings.QueueName, settings.RoutingKey);
        }

        public void BindQueue(string exchangeName, string queueName, string routingKey)
        {
            Chanel.QueueDeclare(queueName, true, false, false);
            Chanel.QueueBind(queueName, exchangeName, routingKey);
        }

        public void DeclareExchange(string exchangeName, string exchangeType)
        {
            Chanel.ExchangeDeclare(exchangeName, exchangeType);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Chanel?.Dispose();
                _connection?.Dispose();
                GC.SuppressFinalize(this);
                _disposed = true;
            }
        }
    }
}

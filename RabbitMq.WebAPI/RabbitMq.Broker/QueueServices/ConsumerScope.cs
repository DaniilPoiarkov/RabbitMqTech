using RabbitMq.Broker.Interfaces;
using RabbitMq.Broker.Models;
using IConnectionFactory = RabbitMQ.Client.IConnectionFactory;

namespace RabbitMq.Broker.QueueServices
{
    public class ConsumerScope : IConsumerScope
    {
        private readonly IConnectionFactory _factory;
        private readonly Lazy<IQueue> _queueLazy;
        private readonly Lazy<IConsumer> _consumerLazy;
        private readonly ScopeSettings _settings;

        private bool _disposed = false; 

        public IConsumer Consumer => _consumerLazy.Value;
        public IQueue Queue => _queueLazy.Value;

        public ConsumerScope(IConnectionFactory factory, ScopeSettings settings)
        {
            _factory = factory;
            _settings = settings;

            _consumerLazy = new(CreateConsumer);
            _queueLazy = new(CreateQueue);
        }

        public IConsumer CreateConsumer() => 
            new Consumer(new()
            {
                Channel = Queue.Chanel,
                QueueName = _settings.QueueName,
            });

        public IQueue CreateQueue() => 
            new Queue(_factory, _settings);

        public void Dispose()
        {
            if (!_disposed)
            {
                _queueLazy.Value?.Dispose();
                GC.SuppressFinalize(this);
                _disposed = true;
            }
        }
    }
}

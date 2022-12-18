using RabbitMq.Broker.Interfaces;
using RabbitMq.Broker.Models;
using RabbitMQ.Client;

namespace RabbitMq.Broker.QueueServices
{
    public class ProducerScope : IProducerScope
    {
        private readonly IConnectionFactory _factory;
        private readonly Lazy<IQueue> _queueLazy;
        private readonly Lazy<IProducer> _producerLazy;
        private readonly ScopeSettings _settings;

        private bool _isDisposed = false;

        public IProducer Producer => _producerLazy.Value;
        public IQueue Queue => _queueLazy.Value;

        public ProducerScope(IConnectionFactory factory, ScopeSettings settings)
        {
            _factory = factory;
            _settings = settings;

            _queueLazy = new(CreateQueue);
            _producerLazy = new(CreateProducer);
        }

        public IProducer CreateProducer() => 
            new Producer(
                new ProducerSettings()
                {
                    Channel = Queue.Chanel,
                    Address = new(
                        _settings.ExchangeType, 
                        _settings.ExchangeName, 
                        _settings.RoutingKey),
                });

        public IQueue CreateQueue() => 
            new Queue(_factory, _settings);

        public void Dispose()
        {
            if (!_isDisposed)
            {
                GC.SuppressFinalize(this);
                Queue?.Dispose();
                _isDisposed = true;
            }
        }
    }
}

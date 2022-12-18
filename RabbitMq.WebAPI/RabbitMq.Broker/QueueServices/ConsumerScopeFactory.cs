using RabbitMq.Broker.Interfaces;
using RabbitMq.Broker.Models;
using RabbitMQ.Client;

namespace RabbitMq.Broker.QueueServices
{
    public class ConsumerScopeFactory : IConsumerScopeFactory
    {
        private readonly IConnectionFactory _connectionFactory;
        public ConsumerScopeFactory(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IConsumerScope Connect(ScopeSettings settings)
        {
            var consumerScope = Open(settings);
            consumerScope.Consumer.Connect();
            return consumerScope;
        }

        public IConsumerScope Open(ScopeSettings settings) => 
            new ConsumerScope(_connectionFactory, settings);
    }
}

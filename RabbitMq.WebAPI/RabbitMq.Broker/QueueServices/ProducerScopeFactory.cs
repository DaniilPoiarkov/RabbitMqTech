using RabbitMq.Broker.Interfaces;
using RabbitMq.Broker.Models;
using RabbitMQ.Client;

namespace RabbitMq.Broker.QueueServices
{
    public class ProducerScopeFactory : IProducerScopeFactory
    {
        private readonly IConnectionFactory _connectionFactory;
        public ProducerScopeFactory(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IProducerScope Open(ScopeSettings settings) => 
            new ProducerScope(_connectionFactory, settings);
    }
}

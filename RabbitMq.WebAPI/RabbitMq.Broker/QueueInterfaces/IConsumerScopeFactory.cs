
using RabbitMq.Broker.Models;

namespace RabbitMq.Broker.Interfaces
{
    public interface IConsumerScopeFactory
    {
        IConsumerScope Open(ScopeSettings settings);
        IConsumerScope Connect(ScopeSettings settings);
    }
}

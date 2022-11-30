
using RabbitMq.Broker.Models;

namespace RabbitMq.Broker.Interfaces
{
    public interface IProducerScopeFactory
    {
        IProducerScope Open(ScopeSettings settings);
    }
}

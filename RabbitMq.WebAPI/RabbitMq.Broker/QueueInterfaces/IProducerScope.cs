
namespace RabbitMq.Broker.Interfaces
{
    public interface IProducerScope : IDisposable
    {
        IProducer Producer { get; }
        IQueue Queue { get; }
        IProducer CreateProducer();
        IQueue CreateQueue();
    }
}


namespace RabbitMq.Broker.Interfaces
{
    public interface IConsumerScope : IDisposable
    {
        IConsumer Consumer { get; }
        IQueue Queue { get; }
        IConsumer CreateConsumer();
        IQueue CreateQueue();
    }
}

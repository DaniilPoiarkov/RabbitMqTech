
using RabbitMQ.Client.Events;

namespace RabbitMq.Broker.Interfaces
{
    public interface IConsumer
    {
        event EventHandler<BasicDeliverEventArgs> Recieved;
        void Connect();
        void SetAcknowledge(ulong deliveryTag, bool processed);
    }
}


using RabbitMQ.Client;

namespace RabbitMq.Broker.Interfaces
{
    public interface IQueue : IDisposable
    {
        IModel Chanel { get; }
        void DeclareExchange(string exchaneName, string exchangeType);
        void BindQueue(string exchaneName, string queueName, string routingKey);
    }
}

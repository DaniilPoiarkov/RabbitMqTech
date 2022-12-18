
namespace RabbitMq.Broker.Interfaces
{
    public interface IProducer
    {
        void Send(string message, string? type = null);
        void SendType(Type type, string message);
    }
}

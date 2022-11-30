using RabbitMQ.Client;

namespace RabbitMq.Broker.Models
{
    public class ProducerSettings
    {
        public IModel Channel { get; set; } = null!;
        public PublicationAddress? Address { get; set; }
    }
}

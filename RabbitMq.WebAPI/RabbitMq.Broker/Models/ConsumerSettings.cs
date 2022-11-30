using RabbitMQ.Client;

namespace RabbitMq.Broker.Models
{
    public class ConsumerSettings
    {
        public string? QueueName { get; set; }
        public bool SequentialFetch { get; set; } = true;
        public bool AutoAcknowledge { get; set; } = false;
        public IModel Channel { get; set; } = null!;
    }
}

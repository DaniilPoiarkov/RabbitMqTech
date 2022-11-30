namespace RabbitMq.Broker.Models
{
    public class ScopeSettings
    {
        public string ExchangeName { get; set; } = null!;
        public string ExchangeType { get; set; } = null!;
        public string? RoutingKey { get; set; }
        public string? QueueName { get; set; }
    }
}

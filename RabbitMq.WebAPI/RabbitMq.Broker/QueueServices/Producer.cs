using RabbitMq.Broker.Interfaces;
using RabbitMq.Broker.Models;
using RabbitMQ.Client;
using System.Text;

namespace RabbitMq.Broker.QueueServices
{
    public class Producer : IProducer
    {
        private readonly ProducerSettings _settings;
        private readonly IBasicProperties _properties;
        public Producer(ProducerSettings settings)
        {
            _settings = settings;
            _properties = _settings.Channel.CreateBasicProperties();
            _properties.Persistent = true;
        }

        public void Send(string message, string? type = null)
        {
            if(!string.IsNullOrEmpty(type))
                _properties.Type = type;

            var body = Encoding.UTF8.GetBytes(message);
            _settings.Channel.BasicPublish(_settings.Address, _properties, body);
        }

        public void SendType(Type type, string message) => 
            Send(message, type.AssemblyQualifiedName);
    }
}

using RabbitMq.Broker.Interfaces;
using RabbitMq.Broker.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMq.Broker.QueueServices
{
    public class Consumer : IConsumer
    {
        private readonly ConsumerSettings _settings;
        private readonly EventingBasicConsumer _consumer;
        public event EventHandler<BasicDeliverEventArgs> Recieved
        {
            add => _consumer.Received += value;
            remove => _consumer.Received -= value;
        }
        public Consumer(ConsumerSettings settings)
        {
            _settings = settings;
            _consumer = new(_settings.Channel);
        }

        public void Connect()
        {
            if (_settings.SequentialFetch)
                _settings.Channel.BasicQos(0, 1, false);

            _settings.Channel.BasicConsume(_settings.QueueName, _settings.AutoAcknowledge, _consumer);
        }

        public void SetAcknowledge(ulong deliveryTag, bool processed)
        {
            if (processed)
                _settings.Channel.BasicAck(deliveryTag, false);
            else
                _settings.Channel.BasicNack(deliveryTag, false, true);
        }
    }
}

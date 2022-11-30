using RabbitMQ.Client;
using System.Text;

namespace RabbitMq.Console.Services
{
    internal class QueueConsoleService
    {
        public static bool SendValue(string message, string exchange, string key)
        {
            Task.Delay(TimeSpan.FromSeconds(2.5)).Wait();

            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange, ExchangeType.Direct);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange, key, null, body);

            return true;
        }
    }
}

using RabbitMq.Console.IoC.Abstract;
using RabbitMq.Console.IoC.Implementations;

namespace RabbitMq.Console.AppBuilder
{
    internal class ConsoleApplicationBuilder
    {
        public readonly ICommandCollection Commands = new CommandCollection();

        private HttpClient _httpClient = new();

        public void ConfigureHttpClient(Action<HttpClient> client) => client.Invoke(_httpClient);
        public void ConfigureHttpClient(HttpClient client) => _httpClient = client;

        public ConsoleApplication Build()
        {
            Commands.AddCommandSingleton(_httpClient);

            return new(Commands.GenerateContainer(), _httpClient);
        }
    }
}

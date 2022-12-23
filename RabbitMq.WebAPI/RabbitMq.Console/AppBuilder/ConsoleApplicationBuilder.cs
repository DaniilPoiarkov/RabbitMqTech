using RabbitMq.Console.AppBuilder.AppContext;
using RabbitMq.Console.AppBuilder.CLI.Abstract;
using RabbitMq.Console.Exceptions;
using RabbitMq.Console.IoC.Abstract;
using RabbitMq.Console.IoC.Implementations;
using System.Data;

namespace RabbitMq.Console.AppBuilder
{
    internal class ConsoleApplicationBuilder
    {
        public readonly ICommandCollection Commands = new CommandCollection();

        public string Title { get; set; } = "Console Client";

        public ConsoleColor ForegroundColor { get; set; } = default;

        private readonly List<Type> _cliCommandTypes = new();

        private readonly List<ICliCommand> _cliCommands = new();

        private readonly List<Func<ConsoleAppContext, ConsoleAppContext>> _middlewares = new();

        private HttpClient _httpClient = new();

        public void ConfigureHttpClient(Action<HttpClient> client) => client.Invoke(_httpClient);
        public void ConfigureHttpClient(HttpClient client) => _httpClient = client;

        public ConsoleApplicationBuilder AddCliCommand<TCliCommand>()
            where TCliCommand : ICliCommand
        {
            _cliCommandTypes.Add(typeof(TCliCommand));
            return this;
        }

        public ConsoleApplicationBuilder AddCliCommand(ICliCommand command)
        {
            _cliCommands.Add(command);
            return this;
        }

        public void Use(Func<ConsoleAppContext, ConsoleAppContext> middleware) => _middlewares.Add(middleware);

        public ConsoleApplication Build()
        {
            ConfigureConsole();

            Commands.AddCommandSingleton(_httpClient);

            var container = Commands.GenerateContainer();

            ImplementCliCommands(container);

            return new(container, _httpClient, _cliCommands, _middlewares);
        }

        private void ConfigureConsole()
        {
            System.Console.Title = Title;
            System.Console.ForegroundColor = ForegroundColor;
        }

        private void ImplementCliCommands(ICommandContainer container)
        {
            for (int i = 0; i < _cliCommandTypes.Count; i++)
            {
                var cliCommandType = _cliCommandTypes[i];
                var parameters = cliCommandType.GetConstructors()
                    .First()
                    .GetParameters()
                    .Select(p => container.GetCommand(p.ParameterType))
                    .ToArray();

                var implementation = Activator.CreateInstance(cliCommandType, parameters);

                if (implementation is null)
                    throw new NullImplementationException($"Cannot create instance of {cliCommandType.Name}");

                _cliCommands.Add((ICliCommand)implementation);
            }
        }
    }
}

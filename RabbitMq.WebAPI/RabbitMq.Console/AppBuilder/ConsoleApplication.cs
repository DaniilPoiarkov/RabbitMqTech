using RabbitMq.Common.Exceptions;
using RabbitMq.Console.Abstract;
using RabbitMq.Console.AppBuilder.AppContext;
using RabbitMq.Console.AppBuilder.CLI.Abstract;
using RabbitMq.Console.Extensions;
using RabbitMq.Console.IoC.Abstract;
using RabbitMq.Console.Services;

namespace RabbitMq.Console.AppBuilder
{
    public class ConsoleApplication
    {
        private readonly ICommandContainer _commandContainer;

        private readonly List<Func<ConsoleAppContext, ConsoleAppContext>> _middlewares;

        private readonly List<ICliCommand> _cliCommands;

        public ConsoleApplication(
            ICommandContainer commandContainer,
            List<ICliCommand> commands,
            List<Func<ConsoleAppContext, ConsoleAppContext>> middlewares)
        {
            _commandContainer = commandContainer;
            _cliCommands = commands;
            _middlewares = middlewares;
        }

        public async Task Run()
        {
            await SetUpUserData();

            while (true)
            {
                try
                {
                    var args = Ext.Ask()
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    var context = SetUpContext(args);
                    ApplyMiddlewares(context);

                    if (context.IsInterrupted || context.CancellationToken.IsCancellationRequested)
                        continue;

                    var command = _cliCommands
                        .FirstOrDefault(cli => cli.ControllerName == context.Args[0]);

                    if (command == null)
                    {
                        System.Console.WriteLine($"No command for \'{context.Args[0]}\' argument");
                        continue;
                    }

                    await command.Execute(context);

                }
                catch(Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }

        internal async Task SetUpUserData()
        {
            var currentUserService = _commandContainer.GetCommand<ICurrentUserService>();
            await currentUserService.Login();
            await currentUserService.SetCurrentUserFromRequest();

            var connectionService = _commandContainer.GetCommand<IHubConnectionService>();
            connectionService.Configure(
                _commandContainer.GetCommand<HttpClient>());
            await connectionService.StartAsync();

            var currentUser = currentUserService.CurrentUser;

            if (currentUser is null)
                throw new UnreachableException();

            System.Console.WriteLine($"Login as:\n\t" +
                $"Id: {currentUser.Id},\n\t" +
                $"Name: {currentUser.Username}\n\t" +
                $"Email: {currentUser.Email}\n\n" +
                $"Write \'help\' to see available commands.");
        }

        private ConsoleAppContext SetUpContext(string[] args)
        {
            return new(
                args,
                _cliCommands,
                _commandContainer);
        }

        private void ApplyMiddlewares(ConsoleAppContext context)
        {
            for (int i = 0; i < _middlewares.Count; i++)
            {
                if (context.IsInterrupted)
                    return;

                context = _middlewares[i].Invoke(context);
            }
        }
    }
}

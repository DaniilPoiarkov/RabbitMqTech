using RabbitMq.Common.DTOs;
using RabbitMq.Common.Exceptions;
using RabbitMq.Console.AppBuilder.CLI.Abstract;
using RabbitMq.Console.IoC.Abstract;
using RabbitMq.Console.Services;

namespace RabbitMq.Console.AppBuilder.AppContext
{
    public class ConsoleAppContext
    {
        public string[] Args { get; internal set; }
        public string EnvUserName { get; }
        public bool IsInterrupted { get; set; } = false;
        public bool IsHandled { get; set; } = false;
        public int ProcessId { get; set; }

        private readonly CancellationTokenSource _tokenSource;
        public CancellationToken CancellationToken { get; }

        public Dictionary<string, string> ContextVariables { get; } = new();
        public UserDto User { get; }
        public List<ICliCommand> CliCommands { get; }
        public ICommandContainer CommandContainer { get; }

        public ConsoleAppContext(string[] args, List<ICliCommand> commands, ICommandContainer container)
        {
            Args = args;
            User = container.GetCommand<CurrentUserService>().CurrentUser ?? throw new UnreachableException();
            CliCommands = commands;
            CommandContainer = container;

            _tokenSource = new();
            CancellationToken = _tokenSource.Token;
            EnvUserName = Environment.UserName;
            ProcessId = Environment.ProcessId;
        }

        public void RequestCancellation() => _tokenSource.Cancel();
    }
}

using RabbitMq.Console.AppBuilder.AppContext;
using RabbitMq.Console.AppBuilder.CLI.Abstract;

namespace RabbitMq.Console.AppBuilder.CLI.Implementations
{
    internal class ClearConsoleCommand : ICliCommand
    {
        public string ControllerName => "clear";

        public string Description => "Clear console window";

        public Task Execute(ConsoleAppContext context)
        {
            var args = context.Args;

            if(args.Length != 1)
            {
                System.Console.WriteLine("No such command");
                return Task.CompletedTask;
            }

            if (args[0] == "clear")
                System.Console.Clear();
            return Task.CompletedTask;

        }
    }
}

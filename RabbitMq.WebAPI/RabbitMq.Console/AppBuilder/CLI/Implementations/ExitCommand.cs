using RabbitMq.Console.AppBuilder.CLI.Abstract;

namespace RabbitMq.Console.AppBuilder.CLI.Implementations
{
    internal class ExitCommand : ICliCommand
    {
        public string ControllerName => "exit";

        public string Description => "Exit an application.";

        public Task Execute(string[] args, ConsoleApplication app)
        {
            if(args.Length == 1 && args[0] == "exit")
                Environment.Exit(0);
            else
                System.Console.WriteLine("No such implementation");

            return Task.CompletedTask;
        }
    }
}

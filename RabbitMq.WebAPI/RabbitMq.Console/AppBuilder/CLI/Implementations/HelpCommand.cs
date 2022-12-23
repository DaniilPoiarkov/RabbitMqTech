
using RabbitMq.Console.AppBuilder.AppContext;
using RabbitMq.Console.AppBuilder.CLI.Abstract;

namespace RabbitMq.Console.AppBuilder.CLI.Implementations
{
    internal class HelpCommand : ICliCommand
    {
        public string ControllerName => "help";
        public string Description => "Displays this message.";

        public Task Execute(ConsoleAppContext context, ConsoleApplication app)
        {
            var args = context.Args;

            if(args.Length == 1 && args[0] == "help")
            {
                for (int i = 0; i < app.CliCommands.Count; i++)
                {
                    var command = app.CliCommands[i];

                    System.Console.WriteLine(
                        command.ControllerName + ":\t" + command.Description + "\n");
                }
            }
            else
                System.Console.WriteLine("No such implementation");

            return Task.CompletedTask;
        }
    }
}

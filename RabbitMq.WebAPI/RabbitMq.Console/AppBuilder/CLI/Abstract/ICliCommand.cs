
using RabbitMq.Console.AppBuilder.AppContext;

namespace RabbitMq.Console.AppBuilder.CLI.Abstract
{
    public interface ICliCommand
    {
        string ControllerName { get; }
        string Description { get; }
        Task Execute(ConsoleAppContext context);
    }
}

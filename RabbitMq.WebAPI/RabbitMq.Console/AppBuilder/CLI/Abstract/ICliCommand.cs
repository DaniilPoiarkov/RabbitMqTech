
namespace RabbitMq.Console.AppBuilder.CLI.Abstract
{
    internal interface ICliCommand
    {
        string ControllerName { get; }
        string Description { get; }
        Task Execute(string[] args, ConsoleApplication app);
    }
}

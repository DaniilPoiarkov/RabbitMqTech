
namespace RabbitMq.Console.IoC.Abstract
{
    public interface ICommandCollection
    {
        ICommandCollection AddCommandTransient<TCommand>();
        ICommandCollection AddCommandTransient<TCommand, TImplementation>()
            where TImplementation : TCommand;

        ICommandCollection AddCommandSingleton<TCommand>();
        ICommandCollection AddCommandSingleton<TCommand>(TCommand implementation);
        ICommandCollection AddCommandSingleton<TCommand, TImplementation>()
            where TImplementation : TCommand;
        ICommandCollection AddCommandSingleton<TCommand, TImplementation>(TImplementation implementation)
            where TImplementation : TCommand;

        ICommandCollection ConfigureOptions<TOptions>(TOptions value)
            where TOptions : class;
        ICommandContainer GenerateContainer();
    }
}

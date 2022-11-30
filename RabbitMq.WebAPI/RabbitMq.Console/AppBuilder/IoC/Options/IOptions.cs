
namespace RabbitMq.Console.IoC.Options
{
    internal interface IOptions<TOptions>
        where TOptions : class
    {
        TOptions Value { get; }
    }
}

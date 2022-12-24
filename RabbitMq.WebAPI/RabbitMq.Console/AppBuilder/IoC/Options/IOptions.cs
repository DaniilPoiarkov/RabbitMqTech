
namespace RabbitMq.Console.IoC.Options
{
    public interface IOptions<TOptions>
        where TOptions : class
    {
        TOptions Value { get; }
    }
}

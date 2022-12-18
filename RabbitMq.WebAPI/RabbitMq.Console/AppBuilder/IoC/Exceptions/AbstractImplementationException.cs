
namespace RabbitMq.Console.Exceptions
{
    public class AbstractImplementationException : Exception
    {
        public AbstractImplementationException(string? message, Exception? ex = null) : base(message, ex) { }
    }
}

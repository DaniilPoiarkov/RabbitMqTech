
namespace RabbitMq.Console.Exceptions
{
    public class NullImplementationException : Exception
    {
        public NullImplementationException(string? message, Exception? ex = null) : base(message, ex) { }
    }
}


namespace RabbitMq.Console.Exceptions
{
    public class NotRegisteredCommandException : Exception
    {
        public NotRegisteredCommandException(string? message, Exception? ex = null) : base(message, ex) { }
    }
}

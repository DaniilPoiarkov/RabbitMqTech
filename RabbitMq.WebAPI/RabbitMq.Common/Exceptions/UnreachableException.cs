
namespace RabbitMq.Common.Exceptions
{
    public class UnreachableException : Exception
    {
        public UnreachableException(string? message, Exception? ex = null) : base(message, ex) { }
    }
}

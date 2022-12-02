
namespace RabbitMq.Common.Exceptions
{
    public class UnreachableException : Exception
    {
        public UnreachableException(string? message = null, Exception? ex = null) : base(message, ex) { }
    }
}

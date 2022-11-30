namespace RabbitMq.Common.Exceptions
{
    public class ServiceImplementationException : Exception
    {
        public ServiceImplementationException(string? message, Exception? ex = null) : base(message, ex) { }
    }
}

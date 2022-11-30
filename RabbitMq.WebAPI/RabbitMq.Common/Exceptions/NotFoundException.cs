
namespace RabbitMq.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entity, Exception? ex = null) : base($"{entity} not found", ex) { }
    }
}

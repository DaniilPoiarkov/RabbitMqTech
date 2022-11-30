
using RabbitMq.Console.IoC.Implementations;

namespace RabbitMq.Console.IoC.Abstract
{
    public interface ICommandContainer
    {
        T GetCommand<T>();
        ICollection<CommandDescriptor> GetAllDescriptors();
    }
}

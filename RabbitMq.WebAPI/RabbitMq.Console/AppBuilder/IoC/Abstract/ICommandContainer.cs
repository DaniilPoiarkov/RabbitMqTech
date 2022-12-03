
using RabbitMq.Console.IoC.Implementations;

namespace RabbitMq.Console.IoC.Abstract
{
    public interface ICommandContainer
    {
        object GetCommand(Type commandType);
        T GetCommand<T>();
        ICollection<CommandDescriptor> GetAllDescriptors();
    }
}

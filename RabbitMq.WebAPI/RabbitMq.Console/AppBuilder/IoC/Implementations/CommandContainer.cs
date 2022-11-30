using RabbitMq.Console.Exceptions;
using RabbitMq.Console.IoC.Abstract;
using RabbitMq.Console.IoC.Enums;
using System.Collections;

namespace RabbitMq.Console.IoC.Implementations
{
    public sealed class CommandContainer : ICommandContainer, IEnumerable<CommandDescriptor>
    {
        private readonly ICollection<CommandDescriptor> _commandDescriptors;
        public CommandContainer(ICollection<CommandDescriptor> descriptors)
        {
            _commandDescriptors = descriptors;
        }

        public ICollection<CommandDescriptor> GetAllDescriptors() =>
            _commandDescriptors;

        public object GetCommand(Type commandType)
        {
            var descriptor = _commandDescriptors
                .SingleOrDefault(d => d.CommandType == commandType);

            if (descriptor == null)
                throw new NotRegisteredCommandException($"Command of type {commandType.Name} isn\'t registered");

            if (descriptor.Implementation != null)
                return descriptor.Implementation;

            var actualType = descriptor.ImplementationType ?? descriptor.CommandType;

            if (actualType.IsAbstract ||
                actualType.IsInterface)
                throw new AbstractImplementationException("Cannot create instance of abstract class or interface");

            var constructorInfo = actualType.GetConstructors().First();

            var parameters = constructorInfo.GetParameters()
                .Select(p => GetCommand(p.ParameterType))
                .ToArray();

            var implementation = Activator.CreateInstance(actualType, parameters);

            if (implementation == null)
                throw new NullImplementationException($"Cannot create instance of {commandType.Name}");

            if (descriptor.Lifetime == CommandLifetime.Singleton)
                descriptor.Implementation = implementation;

            return implementation;
        }

        public T GetCommand<T>() =>
            (T)GetCommand(typeof(T));

        public IEnumerator<CommandDescriptor> GetEnumerator()
        {
            for (int i = 0; i < _commandDescriptors.Count; i++)
            {
                var command = _commandDescriptors.ElementAt(i);
                yield return command;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < _commandDescriptors.Count; i++)
            {
                var command = _commandDescriptors.ElementAt(i);
                yield return command;
            }
        }
    }
}

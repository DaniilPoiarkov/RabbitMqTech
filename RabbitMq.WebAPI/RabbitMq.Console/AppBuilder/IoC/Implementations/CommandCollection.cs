using RabbitMq.Console.Exceptions;
using RabbitMq.Console.IoC.Abstract;
using RabbitMq.Console.IoC.Enums;
using RabbitMq.Console.IoC.Options;

namespace RabbitMq.Console.IoC.Implementations
{
    public sealed class CommandCollection : ICommandCollection
    {
        private readonly List<CommandDescriptor> _commandDescriptors = new();

        public ICommandCollection AddCommandSingleton<TCommand>()
        {
            _commandDescriptors.Add(
                new(typeof(TCommand), CommandLifetime.Singleton));
            return this;
        }

        public ICommandCollection AddCommandSingleton<TCommand>(TCommand implementation)
        {
            if (implementation == null)
                throw new NullImplementationException(
                    $"Command of type {typeof(TCommand).Name} cannot be registered because providen implementation is null");

            _commandDescriptors.Add(
                new(implementation, CommandLifetime.Singleton));
            return this;
        }

        public ICommandCollection AddCommandSingleton<TCommand, TImplementation>()
            where TImplementation : TCommand
        {
            _commandDescriptors.Add(
                new(typeof(TCommand), typeof(TImplementation), CommandLifetime.Singleton));
            return this;
        }

        public ICommandCollection AddCommandSingleton<TCommand, TImplementation>(TImplementation implementation)
            where TImplementation : TCommand
        {
            if (implementation == null)
                throw new NullImplementationException(
                    $"Command of type {typeof(TCommand).Name} cannot be registered because providen implementation is null");

            _commandDescriptors.Add(
                new(typeof(TCommand), implementation, CommandLifetime.Singleton));
            return this;
        }

        public ICommandCollection AddCommandTransient<TCommand>()
        {
            _commandDescriptors.Add(
                new(typeof(TCommand), CommandLifetime.Transient));
            return this;
        }

        public ICommandCollection AddCommandTransient<TCommand, TImplementation>()
            where TImplementation : TCommand
        {
            _commandDescriptors.Add(
                new(typeof(TCommand),
                    typeof(TImplementation),
                    CommandLifetime.Transient));
            return this;
        }

        public ICommandCollection ConfigureOptions<TOptions>(TOptions value)
            where TOptions : class
        {
            _commandDescriptors.Add(
                new(typeof(IOptions<TOptions>),
                    Options<TOptions>.CreateOptions(value),
                    CommandLifetime.Singleton));
            return this;
        }

        public ICommandContainer GenerateContainer() =>
            new CommandContainer(_commandDescriptors);
    }
}

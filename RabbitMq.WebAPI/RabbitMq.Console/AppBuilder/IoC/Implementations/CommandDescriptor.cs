using RabbitMq.Console.IoC.Enums;

namespace RabbitMq.Console.IoC.Implementations
{
    public sealed class CommandDescriptor
    {
        public Type CommandType { get; }
        public Type? ImplementationType { get; }
        public object? Implementation { get; internal set; }
        public CommandLifetime Lifetime { get; }

        public CommandDescriptor(object implementation, CommandLifetime lifetime)
        {
            Implementation = implementation;
            Lifetime = lifetime;
            CommandType = implementation.GetType();
        }

        public CommandDescriptor(Type commandType, CommandLifetime lifetime)
        {
            CommandType = commandType;
            Lifetime = lifetime;
        }

        public CommandDescriptor(Type commandType, Type implementationType, CommandLifetime lifetime)
        {
            CommandType = commandType;
            ImplementationType = implementationType;
            Lifetime = lifetime;
        }

        public CommandDescriptor(Type commandType, object implementation, CommandLifetime lifetime)
        {
            Implementation = implementation;
            Lifetime = lifetime;
            CommandType = commandType;
        }
    }
}

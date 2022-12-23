namespace RabbitMq.Console.AppBuilder.AppContext
{
    public class ConsoleAppContext
    {
        public string[] Args { get; }

        public ConsoleAppContext(string[] args)
        {
            Args = args;
        }
    }
}

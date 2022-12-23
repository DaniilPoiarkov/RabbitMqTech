namespace RabbitMq.Console.AppBuilder.AppContext
{
    public class ConsoleAppContext
    {
        public string[] Args { get; }
        public string EnvUserName { get; }
        public bool IsInterrupted { get; set; } = false;
        public bool IsHandled { get; set; } = false;


        private readonly CancellationTokenSource TokenSource;
        public CancellationToken CancellationToken { get; }

        public Dictionary<string, string> KeyValuePairs { get; } = new();

        public ConsoleAppContext(string[] args)
        {
            Args = args;
            TokenSource = new();
            CancellationToken = TokenSource.Token;
            EnvUserName = Environment.UserName;
        }

        public void RequestCancellation() => TokenSource.Cancel();
    }
}

using RabbitMq.Common.DTOs;

namespace RabbitMq.Console.AppBuilder.AppContext
{
    public class ConsoleAppContext
    {
        public string[] Args { get; }
        public string EnvUserName { get; }
        public bool IsInterrupted { get; set; } = false;
        public bool IsHandled { get; set; } = false;


        private readonly CancellationTokenSource _tokenSource;
        public CancellationToken CancellationToken { get; }

        public Dictionary<string, string> KeyValuePairs { get; } = new();
        public UserDto User { get; }

        public ConsoleAppContext(string[] args, UserDto user)
        {
            Args = args;
            User = user;
            _tokenSource = new();
            CancellationToken = _tokenSource.Token;
            EnvUserName = Environment.UserName;
        }

        public void RequestCancellation() => _tokenSource.Cancel();
    }
}

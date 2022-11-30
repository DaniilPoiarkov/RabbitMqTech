
namespace RabbitMq.Console.IoC.Options
{
    internal class Options<TOptions> : IOptions<TOptions>
        where TOptions : class
    {
        private readonly TOptions _value;
        public TOptions Value => _value;
        private Options(TOptions value)
        {
            _value = value;
        }

        public static IOptions<TValue> CreateOptions<TValue>(TValue value)
            where TValue : class
            => new Options<TValue>(value);
    }
}

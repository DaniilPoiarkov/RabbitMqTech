namespace RabbitMq.Console.Extensions
{
    internal static class Ext
    {
        public static string Ask(string? message = null)
        {
            System.Console.WriteLine(message);
            return System.Console.ReadLine() ?? string.Empty;
        }

        public static TOut ContinueWith<TIn, TOut>(this TIn value, Func<TIn, TOut> action) => 
            action(value);

        public static T Ensure<T, TException>(this T value, Func<T, bool> predicate, TException ex)
            where TException : Exception 
            => predicate(value) ? value : throw ex;
    }
}

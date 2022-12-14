using BenchmarkDotNet.Running;

namespace RebbitMq.BenchmarkTests
{
    public class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<ConsoleClientBenchmark>();
        }
    }
}

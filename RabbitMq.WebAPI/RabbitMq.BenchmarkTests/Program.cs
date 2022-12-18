using BenchmarkDotNet.Running;
using RabbitMq.BenchmarkTests.ConsoleClientTests;

namespace RebbitMq.BenchmarkTests
{
    public class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<ForLoopBenchmark>();
        }
    }
}

using BenchmarkDotNet.Running;
using RebbitMq.BenchmarkTests.ConsoleClientTests;

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

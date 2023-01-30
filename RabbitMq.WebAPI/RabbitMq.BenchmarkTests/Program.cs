using BenchmarkDotNet.Running;
using RabbitMq.BenchmarkTests.Tasks;

namespace RebbitMq.BenchmarkTests;

public class Program
{
    public static void Main()
    {
        BenchmarkRunner.Run<TasksMemoryAllocation>();
    }
}

using BenchmarkDotNet.Attributes;

namespace RabbitMq.BenchmarkTests.Tasks;

[MemoryDiagnoser(false)]
public class TasksMemoryAllocation
{
    private readonly int[] _values = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    [Benchmark]
    public async Task AwaitingWhenAll()
    {
        var list = new List<Task>(_values.Length);

        foreach (var val in _values)
            list.Add(MockAction(val));

        await Task.WhenAll(list);
    }

    [Benchmark]
    public async Task AwaitingWhileCalling()
    {
        foreach(var val in _values)
            await MockAction(val);
    }

    private static async Task<List<Task>> GetTasks()
    {
        var tasks = new List<Task>()
        {
            new(async () => await Task.FromResult(1)),
            new(async () => await Task.FromResult(1)),
            new(async () => await Task.FromResult(1)),
            new(async () => await Task.FromResult(1)),
            new(async () => await Task.FromResult(1)),
            new(async () => await Task.FromResult(1)),
        };

        return await Task.FromResult(tasks);
    }

    private static async Task MockAction(int i)
    {
        await Task.Delay(100 * i);
    }
}

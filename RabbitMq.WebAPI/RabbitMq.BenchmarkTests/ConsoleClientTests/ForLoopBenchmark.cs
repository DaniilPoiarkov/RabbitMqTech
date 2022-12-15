using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RabbitMq.BenchmarkTests.ConsoleClientTests
{
    [MemoryDiagnoser(false)]
    public class ForLoopBenchmark
    {
        [Params(1000, 100_000, 1_000_000)]
        public int Size { get; set; }

        private int[] _items = null!;

        [GlobalSetup]
        public void Setup()
        {
            var rnd = new Random();
            _items = Enumerable.Range(1, Size).Select(i => rnd.Next()).ToArray();
        }

        [Benchmark]
        public void ForLoop_AsArray()
        {
            for (int i = 0; i < Size; i++)
            {
                var item = _items[i];
                MockAction(item);
            }
        }

        [Benchmark]
        public void ForLoop_AsICollection()
        {
            ICollection<int> items = _items;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items.ElementAt(i);
                MockAction(item);
            }
        }

        [Benchmark]
        public void ForLoop_AsIList()
        {
            var items = _items.ToList();

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                MockAction(item);
            }
        }

        [Benchmark]
        public void ForLoop_AsSpan()
        {
            Span<int> items = _items;

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                MockAction(item);
            }
        }

        [Benchmark]
        public void ForLoop_UnsafeAdd()
        {
            Span<int> items = _items;
            ref var searchSpace = ref MemoryMarshal.GetReference(items);

            for (int i = 0; i < items.Length; i++)
            {
                var item = Unsafe.Add(ref searchSpace, i);
                MockAction(item);
            }
        }

        private static int MockAction(int i) => i * i;
    }
}
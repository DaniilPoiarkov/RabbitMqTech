``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.963)
AMD Ryzen 7 5700U with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.100
  [Host]     : .NET 6.0.11 (6.0.1122.52304), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.11 (6.0.1122.52304), X64 RyuJIT AVX2


```
|                Method |    Size |            Mean |         Error |        StdDev | Allocated |
|---------------------- |-------- |----------------:|--------------:|--------------:|----------:|
| **ForLoop_AsICollection** |    **1000** |     **14,893.9 ns** |     **297.00 ns** |     **435.34 ns** |         **-** |
|       ForLoop_AsIList |    1000 |        663.3 ns |      15.02 ns |      43.11 ns |    4056 B |
| **ForLoop_AsICollection** |  **100000** |  **1,440,536.7 ns** |  **21,549.72 ns** |  **17,994.99 ns** |       **1 B** |
|       ForLoop_AsIList |  100000 |    197,486.3 ns |   3,458.30 ns |   4,373.64 ns |  400098 B |
| **ForLoop_AsICollection** | **1000000** | **14,770,702.2 ns** | **212,969.37 ns** | **188,791.80 ns** |      **15 B** |
|       ForLoop_AsIList | 1000000 |  1,235,396.2 ns |  22,513.81 ns |  38,230.16 ns | 4000161 B |

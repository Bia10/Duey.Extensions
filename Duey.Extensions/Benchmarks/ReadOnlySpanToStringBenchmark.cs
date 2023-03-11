using System.Buffers;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Duey.Extensions.Benchmarks;

public class ReadOnlySpanToStringBenchmark
{
    private readonly string _testString;

    public ReadOnlySpanToStringBenchmark()
    {
        _testString = new string(Enumerable.Repeat('a', 100000).ToArray());
    }

    private ReadOnlySpan<char> TestSpan
        => _testString.AsSpan();

    public Summary Execute()
    {
        return BenchmarkRunner.Run<ReadOnlySpanToStringBenchmark>();
    }

    [Benchmark]
    public string ToStringCreate()
    {
        return TestSpan.ToString();
    }

    [Benchmark]
    public string StringBuilderToString()
    {
        StringBuilder stringBuilder = new(TestSpan.Length);
        stringBuilder.Append(TestSpan);
        return stringBuilder.ToString();
    }

    [Benchmark]
    public string FromCharArray()
    {
        return new string(TestSpan.ToArray());
    }

    [Benchmark]
    public string FromSpanAndArrayPool()
    {
        var charArray = ArrayPool<char>.Shared.Rent(TestSpan.Length);
        TestSpan.CopyTo(charArray);
        string result = new(charArray, 0, TestSpan.Length);
        ArrayPool<char>.Shared.Return(charArray);
        return result;
    }
}
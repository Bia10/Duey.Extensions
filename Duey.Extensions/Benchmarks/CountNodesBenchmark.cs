using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using Duey.Extensions.Extensions;

namespace Duey.Extensions.Benchmarks;

public class CountNodesBenchmark
{
    private readonly Dictionary<string, (long totalTime, long totalCount)> Results = new();
    private List<Func<NXNode, int>> CountMethods = null!;
    private IReadOnlyCollection<NXFile> NxFiles = null!;

    [Params(1)]
    public int MaxIterations { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        NxFiles = Directory
            .GetFiles(
                Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "Data (2)"),
                "*.nx")
            .Select(file => new NXFile(file))
            .ToArray();

        CountMethods = new List<Func<NXNode, int>>
        {
            rootNode => rootNode.CountNodesRecursive(),
            rootNode => rootNode.CountSumRecursion(),
            rootNode => rootNode.CountBreadthFirst(),
            rootNode => rootNode.CountStackIteration()
        };
    }

    [Benchmark]
    public void RecursiveCountBenchmark()
    {
        RunBenchmark(CountMethods[0]);
    }

    [Benchmark]
    public void RecursionWithSumCountBenchmark()
    {
        RunBenchmark(CountMethods[1]);
    }

    [Benchmark]
    public void BreadthFirstCountBenchmark()
    {
        RunBenchmark(CountMethods[2]);
    }

    [Benchmark]
    public void StackIterationCountBenchmark()
    {
        RunBenchmark(CountMethods[3]);
    }

    private void RunBenchmark(Func<NXNode, int> countMethod)
    {
        long totalTime = 0;
        long totalCount = 0;

        foreach (var rootNode in NxFiles.Select(static nxFile => nxFile.Root))
            for (var i = 0; i < MaxIterations; i++)
            {
                var sw = Stopwatch.StartNew();
                var count = countMethod(rootNode);
                totalTime += sw.ElapsedMilliseconds;
                totalCount += count;
            }

        Results[countMethod.Method.Name] = (totalTime, totalCount);
    }
}
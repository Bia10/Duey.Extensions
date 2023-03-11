using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using Duey.Extensions.Extensions;

namespace Duey.Extensions.Benchmarks;

public class CountNodesBenchmark
{
    private List<Func<NXNode, int>> CountMethods = null!;

    private IReadOnlyCollection<NXFile> NxFiles = null!;

    [Params(1, 5, 10)]
    private int MaxIterations { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        NxFiles = Directory
            .GetFiles(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "Data (2)"), "*.nx")
            .Select(static file => new NXFile(file))
            .ToArray();

        CountMethods = new List<Func<NXNode, int>>
        {
            static rootNode => rootNode.CountNodesRecursive(),
            static rootNode => rootNode.CountSumRecursion(),
            static rootNode => rootNode.CountBreadthFirst(),
            static rootNode => rootNode.CountStackIteration()
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

        foreach (var nxFile in NxFiles)
        {
            var rootNode = nxFile.Root;

            for (var i = 0; i < MaxIterations; i++)
            for (var j = 0; j < CountMethods.Count; j++)
            {
                var sw = Stopwatch.StartNew();
                var count = countMethod(rootNode);
                totalTime += sw.ElapsedMilliseconds;
                totalCount += count;
            }

            nxFile.Dispose();
        }
    }
}
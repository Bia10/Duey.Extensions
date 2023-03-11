using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Duey.Extensions.Extensions;

namespace Duey.Extensions.Benchmarks;

public class AllNodesBenchmark
{
    private readonly IEnumerable<NXFile> NxFiles;

    public AllNodesBenchmark(IEnumerable<NXFile> nxFiles)
    {
        NxFiles = nxFiles;
    }

    public Summary Execute()
    {
        return BenchmarkRunner.Run<AllNodesBenchmark>();
    }

    [Benchmark]
    public void AllNodesListBreadthFirst()
    {
        foreach (var nxFile in NxFiles) nxFile.AllNodesListBreadthFirst();
    }

    [Benchmark]
    public void AllNodesLinkedListBreathFirst()
    {
        foreach (var nxFile in NxFiles) nxFile.AllNodesLinkedListBreathFirst();
    }

    [Benchmark]
    public void AllNodesListDepthFirst()
    {
        foreach (var nxFile in NxFiles) nxFile.AllNodesListDepthFirst(null);
    }

    [Benchmark]
    public void AllNodesLinkedListDepthFirst()
    {
        foreach (var nxFile in NxFiles) nxFile.AllNodesLinkedListDepthFirst(null);
    }
}
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Duey.Extensions.Benchmarks;
using Plotly.NET;

namespace Duey.Extensions.TestConsole;

internal static class Program
{
    private static void Main()
    {
        var config = DefaultConfig.Instance.WithSummaryStyle(SummaryStyle.Default.WithMaxParameterColumnWidth(50))
            .WithOption(ConfigOptions.StopOnFirstError, false)
            .AddJob(Job.Default.WithWarmupCount(2).WithIterationCount(5).WithMaxRelativeError(0.005))
            .AddDiagnoser(MemoryDiagnoser.Default)
            .AddExporter(MarkdownExporter.GitHub)
            .AddLogger(ConsoleLogger.Default);

        var summary = BenchmarkRunner.Run<CountNodesBenchmark>(config);

        var resultsTime = summary.Reports.Where(static benchmarkReport => benchmarkReport.ResultStatistics is not null)
            .ToDictionary(static benchmarkReport => benchmarkReport.BenchmarkCase.Descriptor.DisplayInfo,
                static benchmarkReport => benchmarkReport.ResultStatistics.Mean);

        var keyValuePairsOrdered = resultsTime.OrderBy(static resultTime => resultTime.Value);

        var methods = new List<string>();
        var times = new List<double>();

        foreach (var kvp in keyValuePairsOrdered)
        {
            methods.Add(kvp.Key);
            times.Add(kvp.Value);
        }

        Chart2D.Chart.Point<string, double, string>(methods, times)
            .WithTraceInfo("Time spent iterating in seconds.", ShowLegend: true)
            .WithTitle($"Benchmark Results of {nameof(CountNodesBenchmark)}")
            .Show();
    }
}
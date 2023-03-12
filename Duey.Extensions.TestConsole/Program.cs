using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Duey.Extensions.Benchmarks;

namespace Duey.Extensions.TestConsole;

internal static class Program
{
    private static void Main()
    {
        var config = DefaultConfig.Instance.WithSummaryStyle(SummaryStyle.Default.WithMaxParameterColumnWidth(50))
            .WithOption(ConfigOptions.StopOnFirstError, true)
            .AddJob(Job.Default.WithWarmupCount(2).WithIterationCount(5).WithMaxRelativeError(0.005))
            .AddDiagnoser(MemoryDiagnoser.Default)
            .AddExporter(MarkdownExporter.GitHub)
            .AddLogger(ConsoleLogger.Default);

        BenchmarkRunner.Run<CountNodesBenchmark>(config);

        // Console.ReadKey();
    }
}
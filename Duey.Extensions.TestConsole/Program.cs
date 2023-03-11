using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Duey.Extensions.Benchmarks;

namespace Duey.Extensions.TestConsole;

internal static class Program
{
    private const string NxFileNamePattern = "*.nx";

    private static readonly string
        UserProfileDirPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    private static readonly string DirectoryPath = Path.Join(UserProfileDirPath, "Downloads", "Data (2)");

    private static IEnumerable<string> FileNames
        => Directory.GetFiles(DirectoryPath, NxFileNamePattern).ToArray();

    private static HashSet<NXFile> NxFileSet
        => LoadNxFiles(FileNames).ToHashSet();

    private static IEnumerable<NXFile> NxFiles
    {
        get => NxFileSet;
        set
        {
            if (!value.Any()) return;

            foreach (var nxFile in value.Select(static nxFile => nxFile))
                NxFileSet.Add(nxFile);
        }
    }

    private static IEnumerable<NXFile> LoadNxFiles(IEnumerable<string> fileNames)
    {
        return fileNames.Select(static file => new NXFile(file));
    }

    private static void Main()
    {
        var config = DefaultConfig.Instance
            .AddJob(Job.Default.WithWarmupCount(2).WithIterationCount(5).WithMaxRelativeError(0.005))
            .AddExporter(new CsvExporter(CsvSeparator.Comma))
            .AddDiagnoser(MemoryDiagnoser.Default);

        BenchmarkRunner.Run<CountNodesBenchmark>(config);

        //  using NXFile nxFile = new(FilePath);
        //var npcRefs = nxFile.AllReferencesToNpcNodesInFile();
        ////var npcTypeNodes = nxFile.GetAllReferencesToNodeTypeInFile(NXNodeType.String);
        //var stopwatch = Stopwatch.StartNew();

        //try
        //{
        //    for (var i = 0; i < 50; i++)
        //    {
        //        stopwatch.Restart();

        //        StringBuilder stringBuilder = new(npcRefs.Count);
        //        foreach (var npcRef in npcRefs)
        //            stringBuilder.Append(npcRef.ReferencingNodeData);

        //        var textString = stringBuilder.ToString();
        //        var textSpan = textString.AsSpan();

        //        //var allReferencedNames = textSpan.TokenizeWithRegex(AnyHyperlinkPrefix()).ToString();
        //        var allReferencedNames2 = textSpan.TokenizeWithRegexCollection2(AllRegexes, false);

        //        stopwatch.Stop();
        //        Console.WriteLine(
        //            $"Run N:{i} run elapsed time: {stopwatch.Elapsed} total/unique allReferencedNames: {allReferencedNames2.Length}/{allReferencedNames2.Distinct().Count()}");
        //    }
        //}
        //finally
        //{
        //    nxFile.Dispose();
        //}
        // Console.ReadKey();
    }
}
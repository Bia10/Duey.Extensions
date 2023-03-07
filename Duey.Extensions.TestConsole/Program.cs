using System.Diagnostics;

namespace Duey.Extensions.TestConsole;

internal static class Program
{
    private static void Main()
    {
        var file = new NXFile(@"C:\Users\Bia\Downloads\Data (2)\Quest.nx");
        var npcRefs = file.ReferencesToNpcNodesInImage("Say");
        //var locationRefs = file.ReferencesToLocationNodesInImage("Say");

        var testTexts = npcRefs.Select(npcRef => npcRef.ReferencingNodeData);
        var allTexts = string.Join(string.Empty, testTexts);
        var textSpan = allTexts.AsSpan();

        Console.WriteLine($"Size of textSpan: {textSpan.Length} chars");

        var stopwatch = Stopwatch.StartNew();
        var mobNameIds = textSpan.TokenizeWithRegex(RegexPatterns.AnyMobName());
        var npcNameIds = textSpan.TokenizeWithRegex(RegexPatterns.AnyNpcName());
        var mapNameIds = textSpan.TokenizeWithRegex(RegexPatterns.AnyMapName());
        stopwatch.Stop();

        Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");
        Console.WriteLine($"Total/Unique mobNameIds: {mobNameIds.Count}/{mobNameIds.Distinct().Count()}," +
                          $" Total/Unique npcNameIds: {npcNameIds.Count}/{npcNameIds.Distinct().Count()}, " +
                          $" Total/Unique mapNameIds: {mapNameIds.Count}/{mapNameIds.Distinct().Count()}, ");
        Console.ReadKey();
    }
}
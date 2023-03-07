using System.Diagnostics;
using System.Text.RegularExpressions;

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

        IEnumerable<Regex> regexCollection = new List<Regex>
        {
            RegexPatterns.AnyMobName(),
            RegexPatterns.AnyNpcName(),
            RegexPatterns.AnyMapName(),
            RegexPatterns.AnyItemName(),
            RegexPatterns.AnyItemName2(),
            RegexPatterns.AnySkillName()
        };

        var stopwatch = Stopwatch.StartNew();

        for (var i = 0; i < 50; i++)
        {
            stopwatch.Restart();

            var allReferencedNames = textSpan
                .TokenizeWithRegexCollection(regexCollection, true)
                .ToList();

            stopwatch.Stop();

            Console.WriteLine(
                $"Run N:{i} run elapsed time: {stopwatch.Elapsed} total/unique allReferencedNames: {allReferencedNames.Count}/{allReferencedNames.Distinct().Count()}");
        }

        Console.ReadKey();
    }
}
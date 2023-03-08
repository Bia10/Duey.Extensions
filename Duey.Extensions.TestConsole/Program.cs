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

        var referencingData = npcRefs.Select(npcRef => npcRef.ReferencingNodeData);
        var textString = string.Join(string.Empty, referencingData);
        var textSpan = textString.AsSpan();

        if (!textSpan.Length.Equals(textString.Length))
            Console.WriteLine("ERROR: textSpan has different size from referencingData!");

        Console.WriteLine($"Size of textString: {textString.Length} chars");
        Console.WriteLine($"Size of textSpan: {textSpan.Length} chars");

        IEnumerable<Regex> regexCollection = new List<Regex>
        {
            RegexPatterns.AnyMobName(),
            RegexPatterns.AnyNpcName(),
            RegexPatterns.AnyMapName(),
            RegexPatterns.AnyItemName(),
            RegexPatterns.AnyItemName2(),
            RegexPatterns.AnyItemPicture(),
            RegexPatterns.AnyItemPicture2(),
            RegexPatterns.AnySkillPicture(),
            RegexPatterns.AnySkillName(),
            RegexPatterns.AnyItemCountInPlayersInv(),
            RegexPatterns.AnyListOpening(),
            RegexPatterns.AnyListClosing(),
            RegexPatterns.AnyPlayerName(),
            RegexPatterns.AnyProgressBar(),
            RegexPatterns.AnyUnknown(),
            RegexPatterns.AnyImageLocation(),
            RegexPatterns.AnyImageLocation2()
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
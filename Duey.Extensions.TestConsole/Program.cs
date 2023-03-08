using System.Diagnostics;
using static Duey.Extensions.RegexPatterns;

namespace Duey.Extensions.TestConsole;

internal static class Program
{
    private static readonly string FilePath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads", "Data (2)", "Quest.nx");

    private static void Main()
    {
        using var nxFile = new NXFile(FilePath);

        var npcRefs = nxFile.AllReferencesToNpcNodesInFile();

        try
        {
            var stopwatch = Stopwatch.StartNew();

            for (var i = 0; i < 50; i++)
            {
                stopwatch.Restart();

                var textString = npcRefs
                    .Select(npcRef => npcRef.ReferencingNodeData)
                    .Aggregate((stringA, stringB) => stringA + stringB);

                var textSpan = textString.AsSpan();

                if (!textSpan.Length.Equals(textString.Length))
                    Console.WriteLine("ERROR: textSpan has different size from original textString!");

                var allReferencedNames2 = textSpan.TokenizeWithRegex(AnyHyperlinkPrefix()).ToString();
                var allReferencedNames = textSpan.TokenizeWithRegexCollection(AllRegexes, false);

                stopwatch.Stop();
                Console.WriteLine(
                    $"Run N:{i} run elapsed time: {stopwatch.Elapsed} total/unique allReferencedNames: {allReferencedNames.Length}/{allReferencedNames.Distinct().Count()}");
            }
        }
        finally
        {
            nxFile.Dispose();
        }

        Console.ReadKey();
    }
}
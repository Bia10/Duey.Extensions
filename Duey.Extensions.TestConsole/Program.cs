//using Duey.Layout;
using System.Diagnostics;
using System.Text;
using static Duey.Extensions.RegexPatterns;

namespace Duey.Extensions.TestConsole;

internal static class Program
{
    private static readonly string FilePath =
        Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "Data (2)",
            "Quest.nx");

    private static void Main()
    {
        using NXFile nxFile = new(FilePath);
        var npcRefs = nxFile.AllReferencesToNpcNodesInFile();
        //var npcTypeNodes = nxFile.GetAllReferencesToNodeTypeInFile(NXNodeType.String);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            for (var i = 0; i < 50; i++)
            {
                stopwatch.Restart();

                StringBuilder stringBuilder = new();
                foreach (var npcRef in npcRefs) stringBuilder.Append(npcRef.ReferencingNodeData);

                var textString = stringBuilder.ToString();
                var textSpan = textString.AsSpan();

                if (!textSpan.Length.Equals(textString.Length))
                    Console.WriteLine("ERROR: textSpan has different size from original textString!");

                //var allReferencedNames2 = textSpan.TokenizeWithRegex(AnyHyperlinkPrefix()).ToString();
                var allReferencedNames = textSpan.TokenizeWithRegexCollection2(AllRegexes, false);

                stopwatch.Stop();
                Console.WriteLine(
                    $"Run N:{i} run elapsed time: {stopwatch.Elapsed} total/unique allReferencedNames: {allReferencedNames.Length}/{allReferencedNames.Distinct().Count()}");
            }
        }
        finally
        {
            nxFile.Dispose();
        }
        // Console.ReadKey();
    }
}
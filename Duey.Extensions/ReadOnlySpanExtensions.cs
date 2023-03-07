using System.Text.RegularExpressions;

namespace Duey.Extensions;

public static class ReadOnlySpanExtensions
{
    public static bool IsEmptyOrWhiteSpace(this ReadOnlySpan<char> textSpan)
    {
        return textSpan.IsEmpty || textSpan.IsWhiteSpace();
    }

    public static List<string> TokenizeWithRegex(this ReadOnlySpan<char> textSpan, Regex regexGenerator,
        bool removeQuotes = true)
    {
        if (textSpan.IsEmptyOrWhiteSpace())
            throw new ArgumentNullException(nameof(textSpan));
        if (regexGenerator is null)
            throw new ArgumentNullException(nameof(regexGenerator));

        var tokens = new List<string>();

        foreach (var valueMatch in regexGenerator.EnumerateMatches(textSpan))
        {
            if (valueMatch.Length.Equals(0))
                continue;

            var firstChar = textSpan[valueMatch.Index];
            var lastChar = textSpan[valueMatch.Index + valueMatch.Length - 1];

            if (removeQuotes && firstChar.Equals('"') && lastChar.Equals('"'))
                tokens.Add(textSpan.Slice(valueMatch.Index + 1, valueMatch.Length - 2).ToString());
            else
                tokens.Add(textSpan.Slice(valueMatch.Index, valueMatch.Length).ToString());
        }

        return tokens;
    }
}
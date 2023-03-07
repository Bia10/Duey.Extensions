using System.Text.RegularExpressions;

namespace Duey.Extensions;

public static class ReadOnlySpanExtensions
{
    public static Match Match(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        return regexGenerator.Match(textSpan.ToString());
    }

    public static MatchCollection MatchCollection(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        return regexGenerator.Matches(textSpan.ToString());
    }

    public static Regex.ValueMatchEnumerator MatchEnumerator(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        return regexGenerator.EnumerateMatches(textSpan);
    }

    public static int MatchCount(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        return regexGenerator.Count(textSpan);
    }

    public static bool HasMatch(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        return regexGenerator.IsMatch(textSpan);
    }

    public static bool IsEmptyOrWhiteSpace(this ReadOnlySpan<char> textSpan)
    {
        return textSpan.IsEmpty || textSpan.IsWhiteSpace();
    }

    public static IEnumerable<string> TokenizeWithRegex(this ReadOnlySpan<char> textSpan, Regex regexGenerator,
        bool removeQuotes = true)
    {
        if (textSpan.IsEmptyOrWhiteSpace())
            throw new ArgumentNullException(nameof(textSpan));
        if (regexGenerator is null)
            throw new ArgumentNullException(nameof(regexGenerator));

        var regexTokens = new List<string>();

        foreach (var valueMatch in textSpan.MatchEnumerator(regexGenerator))
        {
            if (valueMatch.Length.Equals(0))
                continue;

            var firstChar = textSpan[valueMatch.Index];
            var lastChar = textSpan[valueMatch.Index + valueMatch.Length - 1];

            if (removeQuotes && firstChar.Equals('"') && lastChar.Equals('"'))
                regexTokens.Add(textSpan.Slice(valueMatch.Index + 1, valueMatch.Length - 2).ToString());
            else
                regexTokens.Add(textSpan.Slice(valueMatch.Index, valueMatch.Length).ToString());
        }

        return regexTokens;
    }

    public static IEnumerable<string> TokenizeWithRegexCollection(this ReadOnlySpan<char> textSpan,
        IEnumerable<Regex> regexCollection, bool removeQuotes)
    {
        if (textSpan.IsEmptyOrWhiteSpace())
            throw new ArgumentNullException(nameof(textSpan));
        if (regexCollection is null)
            throw new ArgumentNullException(nameof(regexCollection));

        var regexCollectionToken = new List<string>();

        foreach (var regex in regexCollection)
            regexCollectionToken.AddRange(textSpan.TokenizeWithRegex(regex, removeQuotes));

        return regexCollectionToken;
    }
}
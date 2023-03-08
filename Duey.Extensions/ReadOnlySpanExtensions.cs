using System.Text.RegularExpressions;
using DotNext.Buffers;

namespace Duey.Extensions;

public static class ReadOnlySpanExtensions
{
    public static Match MatchRegex(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        if (textSpan.IsEmptyOrWhiteSpace())
            throw new ArgumentNullException(nameof(textSpan));
        if (regexGenerator is null)
            throw new ArgumentNullException(nameof(regexGenerator));

        return regexGenerator.Match(new string(textSpan));
    }

    public static MatchCollection MatchRegexCollection(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        if (textSpan.IsEmptyOrWhiteSpace())
            throw new ArgumentNullException(nameof(textSpan));
        if (regexGenerator is null)
            throw new ArgumentNullException(nameof(regexGenerator));

        return regexGenerator.Matches(new string(textSpan));
    }

    public static Regex.ValueMatchEnumerator EnumerateRegexMatches(this ReadOnlySpan<char> textSpan,
        Regex regexGenerator)
    {
        if (textSpan.IsEmptyOrWhiteSpace())
            throw new ArgumentNullException(nameof(textSpan));
        if (regexGenerator is null)
            throw new ArgumentNullException(nameof(regexGenerator));

        return regexGenerator.EnumerateMatches(textSpan);
    }

    public static int CountRegexMatches(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        if (textSpan.IsEmptyOrWhiteSpace())
            throw new ArgumentNullException(nameof(textSpan));
        if (regexGenerator is null)
            throw new ArgumentNullException(nameof(regexGenerator));

        return regexGenerator.Count(textSpan);
    }

    public static bool HasRegexMatch(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        if (textSpan.IsEmptyOrWhiteSpace())
            throw new ArgumentNullException(nameof(textSpan));
        if (regexGenerator is null)
            throw new ArgumentNullException(nameof(regexGenerator));

        return regexGenerator.IsMatch(textSpan);
    }

    public static bool IsEmptyOrWhiteSpace(this ReadOnlySpan<char> textSpan)
    {
        var trimmed = textSpan.Trim();
        return trimmed.IsEmpty || trimmed.IsWhiteSpace();
    }

    public static ReadOnlySpan<char> TokenizeWithRegex(this ReadOnlySpan<char> textSpan, Regex regexGenerator,
        bool removeQuotes = true)
    {
        if (textSpan.IsEmptyOrWhiteSpace())
            throw new ArgumentNullException(nameof(textSpan));
        if (regexGenerator is null)
            throw new ArgumentNullException(nameof(regexGenerator));

        using var bufferSlim = new BufferWriterSlim<char>(stackalloc char[64]);

        try
        {
            foreach (var valueMatch in textSpan.EnumerateRegexMatches(regexGenerator))
            {
                if (valueMatch.Length.Equals(0)) continue;

                var firstChar = textSpan[valueMatch.Index];
                var lastChar = textSpan[valueMatch.Index + valueMatch.Length - 1];

                if (removeQuotes && firstChar.Equals('"') && lastChar.Equals('"'))
                    bufferSlim.Write(textSpan.Slice(valueMatch.Index + 1, valueMatch.Length - 2));
                else
                    bufferSlim.Write(textSpan.Slice(valueMatch.Index, valueMatch.Length));
            }

            return bufferSlim.ToString();
        }
        finally
        {
            bufferSlim.Dispose();
        }
    }


    public static string TokenizeWithRegexCollection(this ReadOnlySpan<char> textSpan,
        IEnumerable<Regex> regexCollection, bool removeQuotes)
    {
        if (textSpan.IsEmptyOrWhiteSpace())
            throw new ArgumentNullException(nameof(textSpan));
        if (regexCollection is null)
            throw new ArgumentNullException(nameof(regexCollection));

        using var bufferSlim = new BufferWriterSlim<char>(stackalloc char[64]);

        try
        {
            foreach (var regex in regexCollection)
                bufferSlim.Write(textSpan.TokenizeWithRegex(regex, removeQuotes));

            return bufferSlim.ToString();
        }
        finally
        {
            bufferSlim.Dispose();
        }
    }
}
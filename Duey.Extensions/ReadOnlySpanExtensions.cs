using System.Text.RegularExpressions;
using DotNext.Buffers;

namespace Duey.Extensions;

public static class ReadOnlySpanExtensions
{
    public static Match MatchRegex(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        if (textSpan.IsEmptyOrWhiteSpace())
            throw new ArgumentNullException(nameof(textSpan));

        ArgumentNullException.ThrowIfNull(regexGenerator);

        return regexGenerator.Match(new string(textSpan));
    }

    public static MatchCollection MatchRegexCollection(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        if (textSpan.IsEmptyOrWhiteSpace())
            throw new ArgumentNullException(nameof(textSpan));

        ArgumentNullException.ThrowIfNull(regexGenerator);

        return regexGenerator.Matches(new string(textSpan));
    }

    public static Regex.ValueMatchEnumerator EnumerateRegexMatches(this ReadOnlySpan<char> textSpan,
        Regex regexGenerator)
    {
        if (textSpan.IsEmptyOrWhiteSpace())
            throw new ArgumentNullException(nameof(textSpan));

        ArgumentNullException.ThrowIfNull(regexGenerator);

        return regexGenerator.EnumerateMatches(textSpan);
    }

    public static int CountRegexMatches(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        if (textSpan.IsEmptyOrWhiteSpace())
            throw new ArgumentNullException(nameof(textSpan));

        ArgumentNullException.ThrowIfNull(regexGenerator);

        return regexGenerator.Count(textSpan);
    }

    public static bool HasRegexMatch(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        if (textSpan.IsEmptyOrWhiteSpace())
            throw new ArgumentNullException(nameof(textSpan));

        ArgumentNullException.ThrowIfNull(regexGenerator);

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

        ArgumentNullException.ThrowIfNull(regexGenerator);

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

        ArgumentNullException.ThrowIfNull(regexCollection);

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

    public static string TokenizeWithRegexCollection2(this ReadOnlySpan<char> textSpan,
        IEnumerable<Regex> regexCollection,
        bool removeQuotes, int stackAllocThreshold = 8192)
    {
        if (textSpan.IsEmptyOrWhiteSpace())
            throw new ArgumentNullException(nameof(textSpan));

        ArgumentNullException.ThrowIfNull(regexCollection);

        var bufferSize = 0;
        var collection = regexCollection as Regex[] ?? regexCollection.ToArray();
        foreach (var regex in collection)
        foreach (var valueMatch in textSpan.EnumerateRegexMatches(regex))
        {
            if (valueMatch.Length.Equals(0)) continue;

            var firstChar = textSpan[valueMatch.Index];
            var lastChar = textSpan[valueMatch.Index + valueMatch.Length - 1];

            if (removeQuotes && firstChar.Equals('"') && lastChar.Equals('"'))
                bufferSize += valueMatch.Length - 2;
            else
                bufferSize += valueMatch.Length;
        }

        var buffer = bufferSize <= stackAllocThreshold ? stackalloc char[bufferSize] : new char[bufferSize];
        var bufferWriter = new SpanWriter<char>(buffer);

        foreach (var regex in collection)
        foreach (var valueMatch in textSpan.EnumerateRegexMatches(regex))
        {
            if (valueMatch.Length.Equals(0)) continue;

            var firstChar = textSpan[valueMatch.Index];
            var lastChar = textSpan[valueMatch.Index + valueMatch.Length - 1];

            if (removeQuotes && firstChar.Equals('"') && lastChar.Equals('"'))
                bufferWriter.Write(textSpan.Slice(valueMatch.Index + 1, valueMatch.Length - 2));
            else
                bufferWriter.Write(textSpan.Slice(valueMatch.Index, valueMatch.Length));
        }

        return bufferWriter.ToString();
    }
}
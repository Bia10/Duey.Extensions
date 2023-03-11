using System.Text;
using System.Text.RegularExpressions;
using DotNext.Buffers;
using DotNext.Text;
using Duey.Extensions.Exceptions;
using Duey.Extensions.Shared;

namespace Duey.Extensions.Extensions;

public static class ReadOnlySpanExtensions
{
    public static string ToCreateString(this ReadOnlySpan<char> textSpan)
    {
        ArgumentEmptyOrBlankException.ThrowIfEmptyOrBlank(textSpan);

        return string.Create(textSpan.Length, textSpan.ToArray(),
            static (textSpan, resultStr) => textSpan.CopyTo(resultStr));
    }

    public static string ToBuilderString(this ReadOnlySpan<char> textSpan)
    {
        ArgumentEmptyOrBlankException.ThrowIfEmptyOrBlank(textSpan);

        StringBuilder sb = new(textSpan.Length);
        foreach (var @char in textSpan) sb.Append(@char);

        return sb.ToString();
    }

    public static string ToUtf8String(this ReadOnlySpan<char> textSpan)
    {
        ArgumentEmptyOrBlankException.ThrowIfEmptyOrBlank(textSpan);

        var buffer = Encoding.UTF8.GetBytes(textSpan);
        return Encoding.UTF8.GetString(buffer.Span);
    }

    public static Match MatchRegex(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        ArgumentEmptyOrBlankException.ThrowIfEmptyOrBlank(textSpan);
        ArgumentNullException.ThrowIfNull(regexGenerator);

        return regexGenerator.Match(textSpan.ToCreateString());
    }

    public static MatchCollection MatchRegexCollection(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        ArgumentEmptyOrBlankException.ThrowIfEmptyOrBlank(textSpan);
        ArgumentNullException.ThrowIfNull(regexGenerator);

        return regexGenerator.Matches(textSpan.ToCreateString());
    }

    public static Regex.ValueMatchEnumerator EnumerateRegexMatches(this ReadOnlySpan<char> textSpan,
        Regex regexGenerator)
    {
        ArgumentEmptyOrBlankException.ThrowIfEmptyOrBlank(textSpan);
        ArgumentNullException.ThrowIfNull(regexGenerator);

        return regexGenerator.EnumerateMatches(textSpan);
    }

    public static int CountRegexMatches(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        ArgumentEmptyOrBlankException.ThrowIfEmptyOrBlank(textSpan);
        ArgumentNullException.ThrowIfNull(regexGenerator);

        return regexGenerator.Count(textSpan);
    }

    public static bool HasRegexMatch(this ReadOnlySpan<char> textSpan, Regex regexGenerator)
    {
        ArgumentEmptyOrBlankException.ThrowIfEmptyOrBlank(textSpan);
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
        ArgumentEmptyOrBlankException.ThrowIfEmptyOrBlank(textSpan);
        ArgumentNullException.ThrowIfNull(regexGenerator);

        using BufferWriterSlim<char> bufferSlim = new(stackalloc char[8192]);

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
        ArgumentEmptyOrBlankException.ThrowIfEmptyOrBlank(textSpan);
        ArgumentNullException.ThrowIfNull(regexCollection);

        using BufferWriterSlim<char> bufferSlim = new(stackalloc char[8192]);

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
        IEnumerable<Regex> regexCollection, bool removeQuotes, int stackAllocThreshold = 8192)
    {
        ArgumentEmptyOrBlankException.ThrowIfEmptyOrBlank(textSpan);
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

        var allocatedBuffer = bufferSize <= stackAllocThreshold
            ? stackalloc char[bufferSize]
            : new char[bufferSize];
        SpanWriter<char> bufferWriter = new(allocatedBuffer);

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

    public static string TokenizeWithRegexCollection3(this ReadOnlySpan<char> textSpan,
        IEnumerable<Regex> regexCollection, bool removeQuotes)
    {
        ArgumentEmptyOrBlankException.ThrowIfEmptyOrBlank(textSpan);
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

        var sb = StringBuilderSimplePool.Shared.Rent(bufferSize);

        try
        {
            foreach (var regex in collection)
                foreach (var valueMatch in textSpan.EnumerateRegexMatches(regex))
                {
                    if (valueMatch.Length.Equals(0)) continue;

                    var firstChar = textSpan[valueMatch.Index];
                    var lastChar = textSpan[valueMatch.Index + valueMatch.Length - 1];

                    if (removeQuotes && firstChar.Equals('"') && lastChar.Equals('"'))
                        sb.Append(textSpan.Slice(valueMatch.Index + 1, valueMatch.Length - 2));
                    else
                        sb.Append(textSpan.Slice(valueMatch.Index, valueMatch.Length));
                }

            return sb.ToString();
        }
        finally
        {
            StringBuilderSimplePool.Shared.Return(sb);
        }
    }
}
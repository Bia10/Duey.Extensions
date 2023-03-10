using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Duey.Extensions.Exceptions;

[Serializable]
public class ArgumentEmptyOrBlankException : ArgumentException
{
    private const string ArgEmptyOrWhiteSpaceException = "Argument cannot be empty or whitespace only.";

    public ArgumentEmptyOrBlankException()
        : base(ArgEmptyOrWhiteSpaceException)
    {
        HResult = -2147024809;
    }

    public ArgumentEmptyOrBlankException(string? paramName)
        : base(ArgEmptyOrWhiteSpaceException, paramName)
    {
        HResult = -2147024809;
    }

    public ArgumentEmptyOrBlankException(string? message, Exception? innerException)
        : base(message, innerException)
    {
        HResult = -2147024809;
    }

    public ArgumentEmptyOrBlankException(string? paramName, string? message)
        : base(message, paramName)
    {
        HResult = -2147024809;
    }

    protected ArgumentEmptyOrBlankException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public static void ThrowIfEmptyOrBlank(
        ReadOnlySpan<char> argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (!argument.IsEmptyOrWhiteSpace())
            return;

        Throw(paramName);
    }

    public static unsafe void ThrowIfNull(
        [NotNull] void* argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if ((nint)argument != nint.Zero)
            return;

        Throw(paramName);
    }

    #nullable disable
    internal static void ThrowEmptyOrBlankException(
        nint argument,
        [CallerArgumentExpression(nameof(argument))] string paramName = null)
    {
        ArgumentNullException.ThrowIfNull(argument, paramName);

        Throw(paramName);
    }

    [DoesNotReturn]
    internal static void Throw(string paramName)
    {
        throw new ArgumentEmptyOrBlankException(paramName);
    }
}
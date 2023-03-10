using System.Buffers;
using System.Text;

namespace Duey.Extensions.Shared;

internal class StringBuilderArrayPool
{
    private const int SmallPoolSize = 32;
    private const int LargePoolSize = 512;
    private const int MaxPoolSize = LargePoolSize * SmallPoolSize;

    public static readonly StringBuilderArrayPool Shared = new();
    private readonly ArrayPool<char> _largePool;
    private readonly ArrayPool<char> _maxPool;
    private readonly ArrayPool<char> _smallPool;

    private StringBuilderArrayPool()
    {
        _smallPool = ArrayPool<char>.Create(SmallPoolSize, SmallPoolSize);
        _largePool = ArrayPool<char>.Create(LargePoolSize, LargePoolSize);
        _maxPool = ArrayPool<char>.Create(MaxPoolSize, MaxPoolSize);
    }

    public StringBuilder Rent(int minimumCapacity)
    {
        char[] buffer;

        switch (minimumCapacity)
        {
            case <= SmallPoolSize:
                buffer = _smallPool.Rent(SmallPoolSize);
                break;
            case <= LargePoolSize:
                buffer = _largePool.Rent(LargePoolSize);
                break;
            case <= MaxPoolSize:
                buffer = _maxPool.Rent(MaxPoolSize);
                break;
            default:
                return new StringBuilder(minimumCapacity, minimumCapacity);
        }

        return new StringBuilder(minimumCapacity, buffer.Length);
    }

    public void Return(StringBuilder builder)
    {
        switch (builder.Capacity)
        {
            case <= SmallPoolSize:
                _smallPool.Return(builder.ToString().ToCharArray());
                break;
            case <= LargePoolSize:
                _largePool.Return(builder.ToString().ToCharArray());
                break;
            case <= MaxPoolSize:
                _maxPool.Return(builder.ToString().ToCharArray());
                break;
        }
    }
}
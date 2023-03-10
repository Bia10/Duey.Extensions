using System.Text;

namespace Duey.Extensions.Shared;

internal class StringBuilderSimplePool
{
    private const int SmallPoolSize = 32;
    private const int LargePoolSize = 512;
    private const int MaxPoolSize = LargePoolSize * SmallPoolSize;

    public static readonly StringBuilderSimplePool Shared = new();
    private readonly SimplePool<StringBuilder> _largePool;
    private readonly SimplePool<StringBuilder> _maxPool;
    private readonly SimplePool<StringBuilder> _smallPool;

    private StringBuilderSimplePool()
    {
        _smallPool = new SimplePool<StringBuilder>(static () => new StringBuilder(SmallPoolSize));
        _largePool = new SimplePool<StringBuilder>(static () => new StringBuilder(LargePoolSize));
        _maxPool = new SimplePool<StringBuilder>(static () => new StringBuilder(MaxPoolSize));

        for (var i = 0; i < SmallPoolSize; i++) _smallPool.Free(new StringBuilder(SmallPoolSize));
        for (var i = 0; i < LargePoolSize; i++) _largePool.Free(new StringBuilder(LargePoolSize));
        for (var i = 0; i < MaxPoolSize; i++) _maxPool.Free(new StringBuilder(MaxPoolSize));
    }

    public StringBuilder Rent(int minimumCapacity)
    {
        return minimumCapacity switch
        {
            <= SmallPoolSize => _smallPool.Allocate(),
            <= LargePoolSize => _largePool.Allocate(),
            <= MaxPoolSize => _maxPool.Allocate(),
            _ => new StringBuilder(minimumCapacity)
        };
    }

    public void Return(StringBuilder builder)
    {
        switch (builder.Capacity)
        {
            case <= SmallPoolSize:
                builder.Clear();
                _smallPool.Free(builder);
                break;
            case <= LargePoolSize:
                builder.Clear();
                _largePool.Free(builder);
                break;
            case <= MaxPoolSize:
                builder.Clear();
                _maxPool.Free(builder);
                break;
        }
    }
}
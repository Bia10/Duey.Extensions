using System.Collections.Concurrent;

namespace Duey.Extensions.Shared;

internal class SimplePool<T> where T : class
{
    private readonly Func<T> _allocate;
    private readonly ConcurrentStack<T> _values = new();

    public SimplePool(Func<T> allocate)
    {
        _allocate = allocate;
    }

    public T Allocate()
    {
        return _values.TryPop(out var result)
            ? result
            : _allocate();
    }

    public void Free(T value)
    {
        _values.Push(value);
    }
}
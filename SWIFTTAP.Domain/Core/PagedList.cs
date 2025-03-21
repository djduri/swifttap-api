using System.Collections;

namespace SWIFTTAP.Domain.Core;
public sealed class PagedList<T> : IReadOnlyList<T>
{
    private readonly IReadOnlyList<T> _items;
    private readonly int _totalCount;

    public PagedList(IList<T> items, int totalCount)
    {
        _items = items.AsReadOnly();
        _totalCount = totalCount;
    }

    public T this[int index] => _items[index];

    public int Count => _items.Count;
    public int TotalCount => _totalCount;

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
}

namespace SWIFTTAP.Application.Common.DTOs;
public sealed class RangedDTO<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalItemsCount { get; set; }

    public RangedDTO(IEnumerable<T> items, int totalCount)
    {
        Items = items;
        TotalItemsCount = totalCount;
    }
}
using SWIFTTAP.Domain.Cards;

namespace SWIFTTAP.Application.Features.Cards.Links.DTOs;
public sealed class LinkDetailsDTO
{
    public required long Id { get; set; }
    public required string Type { get;  set; }
    public required string Name { get;  set; }
    public required string Url { get;  set; }
    public int? Order { get; set; }
    public required long CardId { get;  set; }
    public required LinkKind LinkKind { get; set; }
    public required bool HasIcon { get; set; }
}

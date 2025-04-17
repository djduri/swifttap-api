using SWIFTTAP.Application.Features.Cards.Links.DTOs;
using SWIFTTAP.Application.Features.Cards.Themes.DTOs;

namespace SWIFTTAP.Application.Features.Cards.Cards.DTOs;
public sealed class CardDetailsDTO
{
    public required long Id { get; set; }
    public required string UserName { get; set; }
    public required string UniqueName { get; set; }
    public required Guid Guid { get; set; }
    public string? Description { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public required bool IsPhoneNumberShareable { get; set; }
    public required bool IsEmailShareable { get; set; }
    public required bool HasLogo { get; set; }
    public required ThemeCardDTO Theme { get; set; }
    public required IList<LinkCardDTO> Links { get; set; }
}

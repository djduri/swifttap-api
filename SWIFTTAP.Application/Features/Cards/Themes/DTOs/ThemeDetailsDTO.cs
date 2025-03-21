namespace SWIFTTAP.Application.Features.Cards.Themes.DTOs;
public sealed class ThemeDetailsDTO
{
    public required long CardId { get;  set; }
    public string? Name { get;  set; }
    public string? PrimaryColor { get;  set; }
    public string? SecondaryColor { get;  set; }
    public required bool HasSharpEdges { get;  set; }
}

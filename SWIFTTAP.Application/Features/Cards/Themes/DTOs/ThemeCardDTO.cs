namespace SWIFTTAP.Application.Features.Cards.Themes.DTOs;
public sealed class ThemeCardDTO
{
    public string? Name { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public required bool HasSharpEdges { get; set; }
}

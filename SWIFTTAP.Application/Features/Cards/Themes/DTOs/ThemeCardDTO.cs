namespace SWIFTTAP.Application.Features.Cards.Themes.DTOs;
public sealed class ThemeCardDTO
{
    public string? Name { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string? TextDark { get; set; }
    public string? TextLight { get; set; }
    public string? Background { get; set; }
    public string? TextOnButtons { get; set; }
    public string? LinkBackgroundColor { get; set; }
    public string? LinkTextColor { get; set; }
    public required bool HasSharpEdges { get; set; }
    public required bool RoundedProfilePicture { get; set; }
    public int? TilesPerRow { get; set; }
}

using SWIFTTAP.Domain.Administration;

namespace SWIFTTAP.Domain.Cards;
public sealed class Theme
{
    public long CardId { get; private set; }
    public Card Card { get; private set; }

    public string? Name { get; private set; }
    public string? PrimaryColor { get; private set; }
    public string? SecondaryColor { get; private set; }
    public string? TextDark { get; private set; }
    public string? TextLight { get; private set; }
    public string? Background { get; private set; }
    public string? TextOnButtons { get; private set; }
    public string? LinkBackgroundColor { get; private set; }
    public string? LinkTextColor { get; private set; }
    public bool HasSharpEdges { get; private set; }
    public bool RoundedProfilePicture { get; private set; }
    public int? TilesPerRow { get; set; }

    public Theme SetName(string? name)
    {
        Name = name;
        return this;
    }

    public Theme SetPrimaryColor(string? primaryColor)
    {
        PrimaryColor = primaryColor;
        return this;
    }

    public Theme SetSecondaryColor(string? secondaryColor)
    {
        SecondaryColor = secondaryColor;
        return this;
    }

    public Theme SetTextDark(string? textDark)
    { 
        TextDark = textDark;
        return this;
    }

    public Theme SetTextLight(string? textLight)
    { 
        TextLight = textLight;
        return this;
    }

    public Theme SetBackground(string? background)
    { 
        Background = background;
        return this;
    }

    public Theme SetLinkBackgroundColor(string? linkBackgroundColor)
    {
        LinkBackgroundColor = linkBackgroundColor;
        return this;
    }
    public Theme SetLinkTextColor(string? linkTextColor)
    {
        LinkTextColor = linkTextColor;
        return this;
    }

    public Theme SetTextOnButtons(string? textOnButtons)
    { 
        TextOnButtons = textOnButtons;
        return this;
    }

    public Theme SetHasSharpEdges(bool hasSharpEdges)
    {
        HasSharpEdges = hasSharpEdges;
        return this;
    }

    public Theme SetRoundedProfilePicture(bool roundedProfilePicture)
    {
        RoundedProfilePicture = roundedProfilePicture;
        return this;
    }

    public Theme SetTilesPerRow(int? tilesPerRow)
    {
        TilesPerRow = tilesPerRow;
        return this;
    }

    internal Theme() 
    {
        HasSharpEdges = false;
    }

    public static class Factory
    {
        public static Theme Create(string name,
                                   string primaryColor,
                                   string secondaryColor,
                                   string textDark,
                                   string textLight,
                                   string background,
                                   string textOnButtons,
                                   bool hasSharpEdges,
                                   string linkBackgroundColor,
                                   string lintTextColor)
        { 
            return new Theme()
                .SetName(name)
                .SetPrimaryColor(primaryColor)
                .SetSecondaryColor(secondaryColor)
                .SetTextDark(textDark)
                .SetTextLight(textLight)
                .SetBackground(background)
                .SetTextOnButtons(textOnButtons)
                .SetHasSharpEdges(hasSharpEdges)
                .SetLinkBackgroundColor(linkBackgroundColor)
                .SetLinkTextColor(lintTextColor);
        }
    }
}

using SWIFTTAP.Domain.Administration;

namespace SWIFTTAP.Domain.Cards;
public sealed class Theme
{
    public long CardId { get; private set; }
    public Card Card { get; private set; }

    public string? Name { get; private set; }
    public string? PrimaryColor { get; private set; }
    public string? SecondaryColor { get; private set; }
    public bool HasSharpEdges { get; private set; }

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

    public Theme SetHasSharpEdges(bool hasSharpEdges)
    {
        HasSharpEdges = hasSharpEdges;
        return this;
    }

    internal Theme() 
    {
        HasSharpEdges = false;
    }

    public static class Factory
    {
        public static Theme Create(string name, string primaryColor, string secondaryColor, bool hasSharpEdges)
        { 
            return new Theme()
                .SetName(name)
                .SetPrimaryColor(primaryColor)
                .SetSecondaryColor(secondaryColor)
                .SetHasSharpEdges(hasSharpEdges);
        }
    }
}

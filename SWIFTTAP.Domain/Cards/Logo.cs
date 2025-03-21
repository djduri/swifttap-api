using SWIFTTAP.Domain.Administration;

namespace SWIFTTAP.Domain.Cards;
public sealed class Logo
{
    public long CardId { get; private set; }
    public Card Card { get; private set; }
    public byte[]? Content { get; private set; }
    public string? ContentType { get; private set; }

    public Logo SetContent(byte[]? content)
    {
        Content = content;
        return this;
    }

    public Logo SetContentType(string? contentType)
    {
        ContentType = contentType;
        return this;
    }

    internal Logo() { }
}

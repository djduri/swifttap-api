using SWIFTTAP.Domain.Base;
using SWIFTTAP.Domain.Messages;

namespace SWIFTTAP.Domain.Cards;

public sealed class LinkIcon
{
    public long LinkId { get; private set; }
    public Link Link { get; private set; }
    public byte[]? Content { get; private set; }
    public string? ContentType { get; private set; }

    public LinkIcon SetLink(Link link)
    {
        if (link == null)
            throw DomainException.FromErrorCode(ErrorCodes.LinkIcon.InvalidLink);

        Link = link;
        LinkId = link.Id;
        return this;
    }

    public LinkIcon SetContent(byte[]? content)
    {
        Content = content;
        return this;
    }

    public LinkIcon SetContentType(string? contentType)
    {
        ContentType = contentType;
        return this;
    }

    internal LinkIcon() { }

    public static class Factory
    {
        public static LinkIcon FromArguments(Link link, byte[]? content = null, string? contentType = null)
        {
            return new LinkIcon()
                .SetLink(link)
                .SetContent(content)
                .SetContentType(contentType);
        }
    }
}

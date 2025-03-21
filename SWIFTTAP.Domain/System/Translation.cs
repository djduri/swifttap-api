using SWIFTTAP.Domain.Base;
using SWIFTTAP.Domain.Common;
using SWIFTTAP.Domain.Extensions;
using SWIFTTAP.Domain.Messages;

namespace SWIFTTAP.Domain.System;

public sealed class Translation : Entity
{
    public string Name { get; private set; }
    public string? Content { get; private set; }
    public Language Language { get; private set; }

    public Translation SetName(string name)
    {
        if (name.IsEmpty())
            throw DomainException.FromErrorCode(ErrorCodes.Translation.InvalidName);

        Name = name;
        return this;
    }

    public Translation SetContent(string? content)
    { 
        Content = content; 
        return this;
    }

    public Translation SetLanguage(Language language)
    {
        if (!Enum.IsDefined(language))
            throw DomainException.FromErrorCode(ErrorCodes.Translation.InvalidLanguage);

        Language = language;
        return this;
    }

    private Translation()
    {
    }

    public static class Factory
    {
        public static Translation Create(string name, Language language)
        {
            return new Translation()
                   .SetName(name)
                   .SetLanguage(language);
        }
    }
}

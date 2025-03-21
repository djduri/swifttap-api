using SWIFTTAP.Domain.Common;

namespace SWIFTTAP.Application.Abstractions.Settings;

public sealed class BrandingSettings : IValidatableSettings
{
    public string? LogoName { get; init; }
    public string? LogoBase64 { get; init; }
    public int LogoWidthInPixels { get; init; }
    public int LogoHeightInPixels { get; init; }
    public string? BackgroundColorHex { get; init; }
    public string? BackgroundTextColorHex { get; init; }
    public string? TextColorHex { get; init; }

    public bool Valid()
    {
        if (string.IsNullOrWhiteSpace(LogoName)) return false;

        if (string.IsNullOrWhiteSpace(LogoBase64)) return false;

        if (string.IsNullOrWhiteSpace(BackgroundColorHex)) return false;

        if (string.IsNullOrWhiteSpace(BackgroundTextColorHex)) return false;

        if (string.IsNullOrWhiteSpace(TextColorHex)) return false;

        if (LogoWidthInPixels == default || LogoWidthInPixels <= 0) return false;

        if (LogoHeightInPixels == default || LogoHeightInPixels <= 0) return false;

        return true;
    }
}

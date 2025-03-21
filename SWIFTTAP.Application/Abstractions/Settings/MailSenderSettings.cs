using SWIFTTAP.Domain.Common;

namespace SWIFTTAP.Application.Abstractions.Settings;

public sealed class MailSenderSettings : IValidatableSettings
{
    public string? Address { get; init; }
    public string? Name { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
    public int SmtpPort { get; init; }
    public string? SmtpHost { get; init; }
    public bool UseSslForConnection { get; init; }

    public bool Valid()
    {
        if (string.IsNullOrWhiteSpace(Address)) return false;

        if (string.IsNullOrWhiteSpace(Name)) return false;

        if (string.IsNullOrWhiteSpace(Username)) return false;

        if (string.IsNullOrWhiteSpace(Password)) return false;

        if (string.IsNullOrWhiteSpace(SmtpHost)) return false;

        if (SmtpPort == default || SmtpPort <= 0) return false;

        return true;
    }
}

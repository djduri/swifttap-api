using SWIFTTAP.Domain.Common;

namespace SWIFTTAP.Application.Abstractions.Settings;

public sealed class EmailTemplatesSettings : IValidatableSettings
{
    public string? Path { get; init; }
    public Dictionary<string, string>? Templates { get; init; } = [];

    public bool Valid()
    {
        if (string.IsNullOrWhiteSpace(Path)) return false;

        if (Templates is null) return false;

        return true;
    }
}

public enum TemplateKey
{
    // Remember to add in appsettings Templates as well
    AuthTwoFactor,
    CreateAdmin,
    ResetPassword,
    ResetPasswordAsAdmin,
    CreateUser,
    RegisterUser,
    ContactForm
}


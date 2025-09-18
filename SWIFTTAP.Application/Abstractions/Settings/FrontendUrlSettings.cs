using SWIFTTAP.Domain.Common;

namespace SWIFTTAP.Application.Abstractions.Settings;

public sealed class FrontendUrlSettings : IValidatableSettings
{
    public string? Url { get; init; }
    public string? ResetPassword { get; init; }
    public string? ConfirmEmail { get; init; }
    public string? GetUserCard { get; init; }
    public string? Auth { get; init; }

    public bool Valid()
    {
        if (string.IsNullOrWhiteSpace(Url)) return false;

        if (string.IsNullOrWhiteSpace(ResetPassword)) return false;

        if (string.IsNullOrWhiteSpace(ConfirmEmail)) return false;

        if (string.IsNullOrWhiteSpace(GetUserCard)) return false;

        //if (string.IsNullOrWhiteSpace(Auth)) return false;

        return true;
    }
}

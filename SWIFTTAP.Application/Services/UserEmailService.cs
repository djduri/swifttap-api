using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;

namespace SWIFTTAP.Application.Services;
internal sealed class UserEmailService : IUserEmailService
{
    private readonly IMailSenderService _mailSenderService;
    private readonly ILogger<UserEmailService> _logger;
    private readonly AuthenticationSettings _authenticationSettings;

    public UserEmailService(IMailSenderService mailSenderService,
                            ILogger<UserEmailService> logger,
                            IOptions<AuthenticationSettings> authenticationSettings)
    {
        _mailSenderService = mailSenderService;
        _logger = logger;
        _authenticationSettings = authenticationSettings.Value;
    }
    public async Task<bool> Send2FaAuthenticationCodeAsync(User user)
    {
        if (string.IsNullOrEmpty(user.Email) || user.UserKeys == null || string.IsNullOrEmpty(user.UserKeys.AuthTwoFactorCode))
        {
            _logger.LogError("Unable to send two-factor authentication code to user with email: {UserEmail}. Missing email or authentication code.", user.Email);
            return false;
        }

        var emailTemplateData = new Dictionary<string, object>
        {
            { "LoginTwoFactorCode", user.UserKeys.AuthTwoFactorCode },
            { "CodeValidityLengthInMinutes", _authenticationSettings.TwoFactorCodeExpirationMinutes },
            { "FirstName", user.Name }
        };

        bool emailSent = await _mailSenderService.SendEmailAsync(user.Email!,
                                                                 TemplateKey.AuthTwoFactor,
                                                                 emailTemplateData);

        if (!emailSent)
        {
            _logger.LogError("Failed to send two-factor authentication code to user: {UserEmail}", user.Email);
            return false;
        }

        _logger.LogInformation("Two-factor authentication code successfully sent to user: {UserEmail}", user.Email);
        return true;
    }
}

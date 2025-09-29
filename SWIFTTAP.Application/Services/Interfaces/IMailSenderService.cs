using Microsoft.AspNetCore.Identity.UI.Services;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Domain.Common;

namespace SWIFTTAP.Application.Services.Interfaces;

public interface IMailSenderService : IEmailSender
{
    public Task<bool> SendEmailAsync(string email, TemplateKey templateKey, IDictionary<string, object> templateVars, Language language = Language.EN);
}

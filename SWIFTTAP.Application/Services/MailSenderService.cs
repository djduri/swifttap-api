using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Stubble.Core.Builders;
using Stubble.Core.Settings;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Common;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SWIFTTAP.Application.Services;

internal sealed class MailSenderService : IMailSenderService
{
    private static readonly Regex _emailTitleRegex = new(@"<title>.*</title>", RegexOptions.Compiled, TimeSpan.FromMilliseconds(1000));

    private readonly MailSenderSettings _senderMailSettings;
    private readonly EmailTemplatesSettings _emailTemplateSettings;
    private readonly BrandingSettings _customizationSettings;
    private readonly ILogger<MailSenderService> _logger;

    public MailSenderService(IOptions<MailSenderSettings> senderMailSettings,
                              IOptions<EmailTemplatesSettings> emailTemplateSettings,
                              IOptions<BrandingSettings> customizationSettings,
                              ILogger<MailSenderService> logger)
    {
        _senderMailSettings = senderMailSettings.Value;
        _emailTemplateSettings = emailTemplateSettings.Value;
        _customizationSettings = customizationSettings.Value;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(string email,
                                           TemplateKey templateKey,
                                           IDictionary<string, object> templateVars,
                                           Language language = Language.EN)
    {
        try
        {
            var templateFileName = GetTemplateFileName(templateKey);
            var (title, htmlMessage) = await RenderEmailTemplateAsync(templateFileName, templateVars, language);

            if (htmlMessage == null || title == null)
            {
                _logger.LogError("Failed to render email content or title for template: {TemplateKey}.", templateKey);
                return false;
            }

            await SendEmailAsync(email, title, htmlMessage);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email for template: {TemplateKey}.", templateKey);
            return false;
        }
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(email, nameof(email));
        ArgumentNullException.ThrowIfNullOrEmpty(subject, nameof(subject));
        ArgumentNullException.ThrowIfNullOrEmpty(htmlMessage, nameof(htmlMessage));

        var mailMessage = CreateMimeMessage(email, subject, htmlMessage);
        await SendEmail(mailMessage);
    }

    private MimeMessage CreateMimeMessage(string email, string subject, string htmlMessage)
    {
        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress(_senderMailSettings.Name, _senderMailSettings.Address));
        mailMessage.To.Add(new MailboxAddress(email, email));
        mailMessage.Subject = subject;
        mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = htmlMessage
        };

        return mailMessage;
    }

    private async Task SendEmail(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();

        try
        {
            client.Connect(_senderMailSettings.SmtpHost, _senderMailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(_senderMailSettings.Username, _senderMailSettings.Password);
            await client.SendAsync(mailMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email via SMTP.");
            throw;  // Propagate the exception to be handled in the caller
        }
        finally
        {
            client.Disconnect(true);
        }
    }

    private async Task<(string? Title, string? HtmlMessage)> RenderEmailTemplateAsync(string templateFileName,
                                                                                   IDictionary<string, object> templateVars,
                                                                                   Language language)
    {
        var languageString = language.ToString().ToLowerInvariant();
        templateVars.Add("LogoName", _customizationSettings.LogoName);
        templateVars.Add("LogoBase64", _customizationSettings.LogoBase64);
        templateVars.Add("LogoWidth", _customizationSettings.LogoWidthInPixels);
        templateVars.Add("LogoHeight", _customizationSettings.LogoHeightInPixels);
        templateVars.Add("TextColor", _customizationSettings.TextColorHex);
        templateVars.Add("BackgroundColor", _customizationSettings.BackgroundColorHex);
        templateVars.Add("BackgroundTextColor", _customizationSettings.BackgroundTextColorHex);

        string file = Path.Combine(_emailTemplateSettings.Path, languageString, templateFileName);
        string content = await File.ReadAllTextAsync(file, Encoding.UTF8);

        var stubble = new StubbleBuilder()
            .Configure(settings => settings.SetIgnoreCaseOnKeyLookup(true).SetMaxRecursionDepth(512))
            .Build();

        var settings = RenderSettings.GetDefaultRenderSettings();
        settings.SkipHtmlEncoding = true;
        settings.CultureInfo = new CultureInfo(languageString);

        var titleMatch = _emailTitleRegex.Match(content);
        var title = titleMatch.Success ? titleMatch.Value.Replace("<title>", string.Empty).Replace("</title>", string.Empty) : null;

        var renderedHtmlMessage = await stubble.RenderAsync(content, templateVars, settings);
        title = title != null ? await stubble.RenderAsync(title, templateVars, settings) : title;

        renderedHtmlMessage = renderedHtmlMessage?.Replace("&#211;", "Ó").Replace("&#243;", "ó");

        return (title, renderedHtmlMessage);
    }

    private string GetTemplateFileName(TemplateKey templateKey)
    {
        if (_emailTemplateSettings.Templates.TryGetValue(templateKey.ToString(), out var templateFileName))
        {
            return templateFileName;
        }
        throw new ArgumentException($"Template not found for key: {templateKey}", nameof(templateKey));
    }
}
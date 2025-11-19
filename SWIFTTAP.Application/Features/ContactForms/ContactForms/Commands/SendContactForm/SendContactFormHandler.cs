using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Features.Cards.Logo.Commands.UpdateLogo;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.ContactForms;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.ContactForms.ContactForms.Commands.SendContactForm
{
    internal sealed class SendContactFormHandler : ICommandHandler<SendContactFormCommand, bool>
    {
        private readonly ContactFormSettings _contactFormSettings;
        private readonly IRepository<ContactForm> _contactFormRepository;
        private readonly ILogger<UpdateLogoHandler> _logger;
        private readonly IMailSenderService _emailSenderService;
        private readonly IUnitOfWork _unitOfWork;

        public SendContactFormHandler(IRepository<ContactForm> contactFormRepository,
                                      ILogger<UpdateLogoHandler> logger,
                                      IMailSenderService emailSenderService,
                                      IOptions<ContactFormSettings> contactFormSettingsOptions,
                                      IUnitOfWork unitOfWork)
        {
            _contactFormRepository = contactFormRepository;
            _logger = logger;
            _emailSenderService = emailSenderService;

            _contactFormSettings = contactFormSettingsOptions.Value;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(SendContactFormCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var createdAt = DateTime.UtcNow;

                await _emailSenderService.SendEmailAsync(_contactFormSettings.RecipientEmail!,
                                                     TemplateKey.ContactForm,
                                                     new Dictionary<string, object>
                                                     {
                                                             { "Name", request.Name },
                                                             { "Email", request.Email },
                                                             { "Phone", request.Phone },
                                                             { "Company", request.Company },
                                                             { "Quantity", request.Quantity },
                                                             { "Message", request.Message },
                                                             { "CreatedAt", createdAt }
                                                     }//,
                                                     /*currentScopeService.GetLanguage()*/);

                if (_contactFormSettings.SaveToDatabase)
                {
                    var contactForm = ContactForm.Factory.Create(request.Name,
                                                         request.Email,
                                                         request.Phone,
                                                         request.Company,
                                                         request.Quantity,
                                                         request.Message,
                                                         createdAt);
                    _contactFormRepository.Add(contactForm);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while handling SendContactFormCommand for Name: {Name}, Email: {Email}.", request.Name, request.Email);
                return false;
            }
        }
    }
}
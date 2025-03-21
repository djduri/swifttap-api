using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Extensions;
using SWIFTTAP.Application.Features.Cards.Cards.Specifications;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Core;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.CreateUser;

internal sealed class CreateUserHandler : ICommandHandler<CreateUserCommand, long>
{
    private readonly UserManager<User> _userManager;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Card> _cardRepository;
    private readonly ILogger<CreateUserHandler> _logger;
    private readonly FrontendUrlSettings _frontendUrlSettings;
    private readonly IMailSenderService _emailSenderService;

    public CreateUserHandler(UserManager<User> userManager,
                               IRepository<User> userRepository,
                               IRepository<Card> cardRepository,
                               ILogger<CreateUserHandler> logger,
                               IOptions<FrontendUrlSettings> frontendUrlSettings,
                               IMailSenderService emailSenderService)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _cardRepository = cardRepository;
        _logger = logger;
        _frontendUrlSettings = frontendUrlSettings.Value;
        _emailSenderService = emailSenderService;
    }

    public async Task<long> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Sprawdzanie, czy użytkownik z podanym e-mailem lub nazwą unikalną już istnieje równolegle
        var emailExistsTask = _userManager.UserExistsByEmailAsync(request.Email);
        var uniqueNameExistsTask = _cardRepository.AnyAsync(new FindCardByUniqueNameSpecification(request.UniqueName), cancellationToken);

        await Task.WhenAll(emailExistsTask, uniqueNameExistsTask);

        if (emailExistsTask.Result)
        {
            _logger.LogWarning("Attempted to create user with an existing email: {Email}.", request.Email);
            throw EntityCreateException.FromErrorCode(ErrorCodes.User.AlreadyExists);
        }

        if (uniqueNameExistsTask.Result)
        {
            _logger.LogWarning("Attempted to create card with an existing unique name: {UniqueName}.", request.UniqueName);
            throw EntityCreateException.FromErrorCode(ErrorCodes.Card.AlreadyExists);
        }

        var newCard = Card.Factory.Create(request.UniqueName);
        _cardRepository.Add(newCard);

        if (newCard is null)
        {
            //logger
            throw EntityCreateException.FromErrorCode(ErrorCodes.Card.CannotCreate);
        }

        // Tworzenie nowego użytkownika
        var newUser = User.Factory.Create(request.Name, request.Email, newCard);

        // Generowanie hasła
        var generatedPassword = await SecretBuilder.GeneratePasswordAsync(null, cancellationToken);

        // Tworzenie linku autoryzacji
        var authLink = new Uri(new Uri(_frontendUrlSettings.Url!), _frontendUrlSettings.Auth).ToString();

        // Wysyłanie e-maila
        if (!await SendRegistrationEmail(request.Email, newUser.Name, generatedPassword, authLink))
            throw EntityCreateException.FromErrorCode(ErrorCodes.User.RegistrationFailed);

        // Rejestracja użytkownika
        var identityResult = await _userManager.CreateAsync(newUser, generatedPassword);

        if (!identityResult.Succeeded)
        {
            // Dodanie logu błędu, gdy rejestracja nie powiedzie się
            _logger.LogError("Registration failed for user with email: {Email}. Errors: {Errors}.", request.Email, string.Join(", ", identityResult.Errors.Select(e => e.Description)));
            throw EntityCreateException.FromErrorCode(ErrorCodes.User.RegistrationFailed);
        }

        // Dodanie roli użytkownika
        await _userManager.AddToRoleAsync(newUser, Authorization.Roles.User.ToString());

        return newUser.Id;
    }

    private async Task<bool> SendRegistrationEmail(string email, string userName, string generatedPassword, string authLink)
    {
        try
        {
            var emailSentSuccessful = await _emailSenderService.SendEmailAsync(
                email,
                TemplateKey.CreateUser,
                new Dictionary<string, object>
                {
                    { "Name", userName },
                    { "Password", generatedPassword },
                    { "AuthLink", authLink }
                },
                Domain.Common.Language.EN);

            return emailSentSuccessful;
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to send registration email to {Email}: {Error}.", email, ex.Message);
            return false;
        }
    }
}

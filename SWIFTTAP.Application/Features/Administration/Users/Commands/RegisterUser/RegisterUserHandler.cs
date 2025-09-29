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
using SWIFTTAP.Domain.Extensions;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;
using System.Net;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.RegisterUser;

internal sealed class RegisterUserHandler : ICommandHandler<RegisterUserCommand, long>
{
    private readonly UserManager<User> _userManager;
    private readonly IMailSenderService _emailSenderService;
    private readonly IRepository<Card> _cardRepository;
    private readonly ILogger<RegisterUserHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly FrontendUrlSettings _frontendUrlSettings;
    private readonly ICurrentScopeService _currentScopeService;

    public RegisterUserHandler(UserManager<User> userManager,
                               IMailSenderService emailSenderService,
                               IRepository<Card> cardRepository,
                               IOptions<FrontendUrlSettings> frontendUrlSettings,
                               ILogger<RegisterUserHandler> logger,
                               IUnitOfWork unitOfWork,
                               ICurrentScopeService currentScopeService)
    {
        _userManager = userManager;
        _emailSenderService = emailSenderService;
        _cardRepository = cardRepository;
        _frontendUrlSettings = frontendUrlSettings.Value;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _currentScopeService = currentScopeService;
    }

    public async Task<long> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Sprawdzanie, czy użytkownik z podanym e-mailem już istnieje
        if (await _userManager.UserExistsByEmailAsync(request.Email))
        {
            _logger.LogWarning("Attempted registration with an existing email: {Email}.", request.Email);
            throw EntityCreateException.FromErrorCode(ErrorCodes.User.AlreadyExists);
        }

        // Sprawdzanie, czy użytkownik z podaną nazwą unikalną już istnieje
        if (await _cardRepository.AnyAsync(new FindCardByUniqueNameSpecification(request.UniqueName), cancellationToken))
        {
            _logger.LogWarning("Attempted registration with an existing unique name: {UniqueName}.", request.UniqueName);
            throw EntityCreateException.FromErrorCode(ErrorCodes.User.AlreadyExists);
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

        // Rejestracja użytkownika
        var identityResult = await _userManager.CreateAsync(newUser, request.Password);

        if (!identityResult.Succeeded)
        {
            // Dodanie logu błędu, gdy rejestracja nie powiedzie się
            _logger.LogError("Registration failed for user with email: {Email}. Errors: {Errors}.", request.Email, string.Join(", ", identityResult.Errors.Select(e => e.Description)));
            throw EntityCreateException.FromErrorCode(ErrorCodes.User.RegistrationFailed);
        }

        // Dodanie roli użytkownika
        await _userManager.AddToRoleAsync(newUser, Authorization.Roles.User.ToString());

        var token = (await _userManager.GenerateEmailConfirmationTokenAsync(newUser)).EncodeToBase64();

        var confirmationUrl = (_frontendUrlSettings.Url + _frontendUrlSettings.ConfirmEmail).Replace("{token}", token)
                                                                                            .Replace("{userEmail}", newUser.Email);

        await _emailSenderService.SendEmailAsync(newUser.Email!,
                                                 TemplateKey.RegisterUser,
                                                 new Dictionary<string, object>
                                                 {
                                                             { "Name", newUser.Name },
                                                             { "ConfirmationUrl",  confirmationUrl},
                                                 }//,
                                                 /*currentScopeService.GetLanguage()*/);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newUser.Id;
    }
}

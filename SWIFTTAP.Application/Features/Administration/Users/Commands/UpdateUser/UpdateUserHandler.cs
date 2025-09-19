using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Administration.Users.Specifications;
using SWIFTTAP.Application.Features.Cards.Cards.Specifications;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.UpdateUser;

internal sealed class UpdateUserHandler : ICommandHandler<UpdateUserCommand, long>
{
    private readonly UserManager<User> _userManager;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Card> _cardRepository;
    private readonly ILogger<UpdateUserHandler> _logger;
    private readonly IUserService _userService;

    public UpdateUserHandler(UserManager<User> userManager,
                             IRepository<User> userRepository,
                             IRepository<Card> cardRepository,
                             ILogger<UpdateUserHandler> logger,
                             IUserService userService)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _cardRepository = cardRepository;
        _logger = logger;
        _userService = userService;
    }

    public async Task<long> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // Sprawdzenie uprawnień
        if (!_userService.HasAuthUserPermissionToUser(request.Id))
        {
            _logger.LogWarning("Attempted to update user with ID {TargetUserId} without sufficient permissions.", request.Id);
            throw AuthorizationException.FromErrorCode(ErrorCodes.Application.AccessDenied);
        }

        var user = await _userRepository.GetAsync(new FindUserWithCardSpecification(request.Id), cancellationToken) ??
            throw EntityNotFoundException.FromErrorCode(ErrorCodes.User.NotFound);

        // Sprawdzanie, czy użytkownik z podaną nazwą unikalną już istnieje
        if (user.Card.UniqueName != request.UniqueName)
        {
            if (await _cardRepository.AnyAsync(new FindCardByUniqueNameSpecification(request.UniqueName), cancellationToken))
            {
                _logger.LogWarning("Attempted to update user card with an existing unique name: {UniqueName}.", request.UniqueName);
                throw EntityCreateException.FromErrorCode(ErrorCodes.User.AlreadyExists);
            }
            user.Card.SetUniqueName(request.UniqueName);
        }              

        // Ustawienie nowych wartości dla użytkownika
        user.SetName(request.Name)
            .SetTwoFactorEnabled(request.TwoFactorEnabled);

        // Aktualizacja użytkownika
        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            _logger.LogError("Failed to update user with email: {Email}. Errors: {Errors}.", user.Email, string.Join(", ", updateResult.Errors.Select(e => e.Description)));
            throw EntityCreateException.FromErrorCode(ErrorCodes.User.UpdateFailed);
        }               

        return user.Id;
    }
}

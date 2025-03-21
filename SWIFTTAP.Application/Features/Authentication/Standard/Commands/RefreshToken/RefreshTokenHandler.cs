using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Common.DTOs;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.RefreshToken;

internal sealed class RefreshTokenHandler : ICommandHandler<RefreshTokenCommand, TokenDTO>
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<RefreshTokenHandler> _logger;
    private readonly DatabaseContext _databaseContext;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenHandler(ITokenService tokenService,
                               ILogger<RefreshTokenHandler> logger,
                               DatabaseContext databaseContext,
                               IUnitOfWork unitOfWork)
    {
        _tokenService = tokenService;
        _logger = logger;
        _databaseContext = databaseContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<TokenDTO> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _databaseContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.Value == request.RefreshToken, cancellationToken);

        if (refreshToken is null)
        {
            _logger.LogInformation("Invalid refresh token: {refreshToken}", request.RefreshToken);
            throw AuthenticationException.FromErrorCode(ErrorCodes.Authentication.Failed);
        }

        var userId = await _tokenService.ValidateRefreshTokenAndGetUserIdAsync(request.RefreshToken, cancellationToken);

        var user = await _databaseContext.Users.Include(x => x.Card).SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);
        if (user is null || user.Id != refreshToken.UserId)
        {
            _logger.LogInformation("Invalid user for refresh token: {userId}", userId);
            throw AuthenticationException.FromErrorCode(ErrorCodes.Authentication.Failed);
        }

        var token = await _tokenService.CreateTokenWithRefreshTokenAsync(user, cancellationToken);

        // Revoke the old refresh token and save changes once
        refreshToken.Revoke();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return token;
    }
}

using Microsoft.AspNetCore.Identity;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Administration.Users.Specifications;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Common;
using SWIFTTAP.Domain.Core;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.ResetPasswordAsAdmin;
internal sealed class ResetPasswordAsAdminHandler : ICommandHandler<ResetPasswordAsAdminCommand, long>
{
    private readonly UserManager<User> _userManager;
    private readonly IMailSenderService _emailSenderService;
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ResetPasswordAsAdminHandler(UserManager<User> userManager,
                                       IMailSenderService emailSenderService,
                                       IRepository<User> userRepository,
                                       IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _emailSenderService = emailSenderService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(ResetPasswordAsAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(new FindUserWithNonRevokedRefreshTokensSpecification(request.Id), cancellationToken) ??
            throw EntityUpdateException.FromErrorCode(ErrorCodes.User.NotFound);

        var password = await SecretBuilder.GeneratePasswordAsync(null, cancellationToken);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        await _userManager.ResetPasswordAsync(user, token, password);

        //TODO: zmienić język maila na język domyslny z global variables
        await _emailSenderService.SendEmailAsync(user.Email!,
                                                 TemplateKey.ResetPasswordAsAdmin,
                                                 new Dictionary<string, object>
                                                 {
                                                     { "FirstName", user.Name },
                                                     { "Password", password },
                                                 },
                                                 Language.PL);

        // revoke all refresh tokens
        user.RevokeAllRefreshTokens();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Abstractions.Settings;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Administration.Users.Specifications;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Extensions;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.ResetPassword;
internal sealed class ResetPasswordHandler : ICommandHandler<ResetPasswordCommand, long>
{
    private readonly UserManager<User> _userManager;
    private readonly IMailSenderService _emailSenderService;
    private readonly FrontendUrlSettings _frontendUrlSettings;
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentScopeService _currentScopeService;

    public ResetPasswordHandler(UserManager<User> userManager,
                                IMailSenderService emailSenderService,
                                IOptions<FrontendUrlSettings> frontendUrlSettings,
                                IRepository<User> userRepository,
                                IUnitOfWork unitOfWork,
                                ICurrentScopeService currentScopeService)
    {
        _userManager = userManager;
        _emailSenderService = emailSenderService;
        _frontendUrlSettings = frontendUrlSettings.Value;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _currentScopeService = currentScopeService;
    }

    public async Task<long> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(new FindUserWithNonRevokedRefreshTokensSpecification(request.Email), cancellationToken) ??
            throw EntityUpdateException.FromErrorCode(ErrorCodes.Application.AccessDenied);

        var token = (await _userManager.GeneratePasswordResetTokenAsync(user)).EncodeToBase64();

        var resetUrl = (_frontendUrlSettings.Url + _frontendUrlSettings.ResetPassword).Replace("{token}", token)
                                                                                      .Replace("{userEmail}", user.Email);

        await _emailSenderService.SendEmailAsync(user.Email!,
                                                 TemplateKey.ResetPassword,
                                                 new Dictionary<string, object>
                                                 {
                                                     { "FirstName", user.Name },
                                                     { "ResetUrl",  resetUrl},
                                                 });
                                                 //_currentScopeService.GetLanguage());

        // revoke all refresh tokens
        user.RevokeAllRefreshTokens();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
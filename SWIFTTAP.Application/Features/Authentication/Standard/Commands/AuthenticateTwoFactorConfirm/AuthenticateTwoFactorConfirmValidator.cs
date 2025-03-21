using FluentValidation;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.AuthenticateTwoFactorConfirm;
public sealed class AuthenticateTwoFactorConfirmValidator : AbstractValidator<AuthenticateTwoFactorConfirmCommand>
{
    public AuthenticateTwoFactorConfirmValidator()
    {
        RuleFor(x => x.Username).NotEmpty().EmailAddress();
        RuleFor(x => x.AuthTwoFactorKey).NotEmpty();
        RuleFor(x => x.AuthTwoFactorCode).NotEmpty();
    }
}

using FluentValidation;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.CreateAndSendTwoFactorCode;
public sealed class CreateAndSendTwoFactorCodeValidator : AbstractValidator<CreateAndSendTwoFactorCodeCommand>
{
    public CreateAndSendTwoFactorCodeValidator()
    {
        RuleFor(x => x.Username).NotEmpty().EmailAddress();
        RuleFor(x => x.AuthTwoFactorKey).NotEmpty();
    }
}

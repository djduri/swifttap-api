using FluentValidation;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.ResetPasswordConfirm;

public sealed class ResetPasswordConfirmValidator : AbstractValidator<ResetPasswordConfirmCommand>
{
    public ResetPasswordConfirmValidator()
    {
        RuleFor(x => x.Email).IsEmail();
        RuleFor(x => x.NewPassword).NotEmpty();
        RuleFor(x => x.Token).NotEmpty();
    }
}

using FluentValidation;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.ChangePassword;
public sealed class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty();
    }
}

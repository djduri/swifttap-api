using FluentValidation;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.RegisterConfirm;

public sealed class RegisterConfirmValidator : AbstractValidator<RegisterConfirmCommand>
{
    public RegisterConfirmValidator()
    {
        RuleFor(x => x.Email).IsEmail();
        RuleFor(x => x.Token).NotEmpty();
    }
}

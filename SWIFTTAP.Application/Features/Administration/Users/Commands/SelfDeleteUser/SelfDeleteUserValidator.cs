using FluentValidation;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.SelfDeleteUser;
public sealed class SelfDeleteUserValidator : AbstractValidator<SelfDeleteUserCommand>
{
    public SelfDeleteUserValidator()
    {
        RuleFor(x => x.Email).MaximumLength(255).EmailAddress();
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(255);
    }
}

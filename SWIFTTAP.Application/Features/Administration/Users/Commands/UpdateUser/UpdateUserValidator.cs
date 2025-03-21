using FluentValidation;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.UpdateUser;
public sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.UniqueName).NotEmpty().MaximumLength(100);
    }
}

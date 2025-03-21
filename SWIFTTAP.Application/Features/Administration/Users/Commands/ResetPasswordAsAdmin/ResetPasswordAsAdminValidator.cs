using FluentValidation;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.ResetPasswordAsAdmin;
public sealed class ResetPasswordAsAdminValidator : AbstractValidator<ResetPasswordAsAdminCommand>
{
    public ResetPasswordAsAdminValidator()
    {
        RuleFor(x => x.Id).IsIdentifier();
    }
}

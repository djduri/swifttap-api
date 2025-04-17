using FluentValidation;
using SWIFTTAP.Application.Common;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Administration.DeletedUsers.Command.DeleteDeletedUser;
public sealed class DeleteDeletedUserValidator : AbstractValidator<DeleteDeletedUserCommand>
{
    public DeleteDeletedUserValidator()
    {
        RuleFor(x => x.Id).IsIdentifier();
    }
}

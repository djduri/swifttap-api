using FluentValidation;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Cards.Links.Commands.DeleteLink;
public sealed class DeleteLinkValidator : AbstractValidator<DeleteLinkCommand>
{
    public DeleteLinkValidator()
    {
        RuleFor(x => x.Id).IsIdentifier();
    }
}

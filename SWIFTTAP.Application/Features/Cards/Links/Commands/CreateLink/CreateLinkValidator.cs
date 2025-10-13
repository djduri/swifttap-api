using FluentValidation;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Cards.Links.Commands.CreateLink;
public sealed class CreateLinkValidator : AbstractValidator<CreateLinkCommand>
{
    public CreateLinkValidator()
    {
        RuleFor(x => x.CardId).IsIdentifier();
        RuleFor(x => x.Type).NotEmpty().MaximumLength(8);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Url).NotEmpty().MaximumLength(512);
        RuleFor(x => x.Order).GreaterThan(0);
        RuleFor(x => x.LinkKind).IsInEnum();
    }
}

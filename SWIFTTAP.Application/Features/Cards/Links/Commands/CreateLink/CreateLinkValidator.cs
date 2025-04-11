using FluentValidation;
using SWIFTTAP.Application.Common;

namespace SWIFTTAP.Application.Features.Cards.Links.Commands.CreateLink;
public sealed class CreateLinkValidator : AbstractValidator<CreateLinkCommand>
{
    public CreateLinkValidator()
    {
        RuleFor(x => x.Type).NotEmpty().MaximumLength(8);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(32);
        RuleFor(x => x.Url).NotEmpty().MaximumLength(512);
        RuleFor(x => x.Order).GreaterThan(0);
    }
}

using FluentValidation;

namespace SWIFTTAP.Application.Features.Cards.Links.Commands.UpdateLink;
public sealed class UpdateLinkValidator : AbstractValidator<UpdateLinkCommand>
{
    public UpdateLinkValidator()
    {
        RuleFor(x => x.Type).NotEmpty().MaximumLength(8);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Url).NotEmpty().MaximumLength(512);
        RuleFor(x => x.Order).GreaterThan(0);
    }
}

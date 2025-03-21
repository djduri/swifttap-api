using FluentValidation;

namespace SWIFTTAP.Application.Features.Cards.Cards.Queries.GetCard;
public sealed class GetCardValidator : AbstractValidator<GetCardQuery>
{
    public GetCardValidator()
    {
        RuleFor(x => x.Guid).NotEmpty();
    }
}

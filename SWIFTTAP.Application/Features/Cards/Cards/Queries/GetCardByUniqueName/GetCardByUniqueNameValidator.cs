using FluentValidation;

namespace SWIFTTAP.Application.Features.Cards.Cards.Queries.GetCardByUniqueName;
public sealed class GetCardByUniqueNameValidator : AbstractValidator<GetCardByUniqueNameQuery>
{
    public GetCardByUniqueNameValidator()
    {
        RuleFor(x => x.UniqueName).NotEmpty().MaximumLength(100);
    }
}

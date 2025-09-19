using FluentValidation;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Cards.Cards.Commands.UpdateCard;
public sealed class UpdateCardValidator : AbstractValidator<UpdateCardCommand>
{
    public UpdateCardValidator()
    {
        RuleFor(x => x.CardId).IsIdentifier();
        RuleFor(x => x.Description).MaximumLength(500);

        // Walidacja adresu email (jeśli nie null)
        When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(x => x.Email).MaximumLength(255).EmailAddress();
        });

        // Walidacja numeru telefonu (jeśli nie null)
        When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber), () =>
        {
            RuleFor(x => x.PhoneNumber).MaximumLength(25)
                                       .Matches(@"^\+?[0-9\s\-\(\)]{6,25}$");
        });
    }
}

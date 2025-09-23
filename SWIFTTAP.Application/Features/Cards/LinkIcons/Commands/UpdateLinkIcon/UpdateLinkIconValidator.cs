using FluentValidation;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Cards.LinkIcons.Commands.UpdateLinkIcon;

public sealed class UpdateLinkIconValidator : AbstractValidator<UpdateLinkIconCommand>
{
    public UpdateLinkIconValidator()
    {
        RuleFor(x => x.LinkId).IsIdentifier();
        When(x => x.File is not null, () =>
        {            
            RuleFor(x => x.File).NotNull();
            RuleFor(x => x.File!.ContentType)
                .Matches(@"^image\/(jpeg|png|bmp)$")
                .WithMessage("Only image files (JPEG, PNG, BMP) are allowed.");
            RuleFor(x => x.File!.Length)
                .LessThanOrEqualTo(5 * 1024 * 1024)
                .WithMessage("The file size must be less than or equal to 5MB.");
        });
    }
}

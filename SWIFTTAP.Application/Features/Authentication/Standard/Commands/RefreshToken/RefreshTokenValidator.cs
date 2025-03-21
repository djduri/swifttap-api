using FluentValidation;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.RefreshToken;
public sealed class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

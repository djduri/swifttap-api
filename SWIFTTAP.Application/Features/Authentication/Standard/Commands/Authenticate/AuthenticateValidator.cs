using FluentValidation;

namespace SWIFTTAP.Application.Features.Authentication.Standard.Commands.Authenticate;
public sealed class AuthenticateValidator : AbstractValidator<AuthenticateCommand>
{
    public AuthenticateValidator()
    {
        RuleFor(x => x.Username).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

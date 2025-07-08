using FluentValidation;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Administration.Users.Commands.RegisterUser;
public sealed class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Email).IsEmail();
        RuleFor(x => x.UniqueName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Password).NotEmpty();
        //RuleFor(x => x.Password)
        //            .NotEmpty()
        //            .MinimumLength(8)
        //            .Matches("[A-Z]")
        //            .Matches("[a-z]")
        //            .Matches("[0-9]")
        //            .Matches("[^a-zA-Z0-9]");
    }
}

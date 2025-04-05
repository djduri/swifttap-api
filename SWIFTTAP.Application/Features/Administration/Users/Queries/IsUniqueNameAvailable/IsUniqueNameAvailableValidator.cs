using FluentValidation;

namespace SWIFTTAP.Application.Features.Administration.Users.Queries.IsUniqueNameAvailable;
public sealed class IsUniqueNameAvailableValidator : AbstractValidator<IsUniqueNameAvailableQuery>
{
    public IsUniqueNameAvailableValidator()
    {
        RuleFor(x => x.UniqueName).NotEmpty().MaximumLength(100);
    }
}

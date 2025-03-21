using FluentValidation;

namespace SWIFTTAP.Application.Common;
public sealed class SortingArgumentsValidator : AbstractValidator<SortingArguments>
{
    public SortingArgumentsValidator()
    {
        RuleFor(x => x.SortBy).NotEmpty();
    }
}

using FluentValidation;

namespace SWIFTTAP.Application.Common;

public sealed class PaginationArgumentsValidator : AbstractValidator<PaginationArguments>
{
    public PaginationArgumentsValidator()
    {
        RuleFor(x => x.StartNumber).NotNull().GreaterThanOrEqualTo(0);
        RuleFor(x => x.NumberOfRecords).NotEmpty().GreaterThan(0);
    }
}


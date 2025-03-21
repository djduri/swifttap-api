using FluentValidation;
using SWIFTTAP.Application.Common;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Statistics.LinkVisitStatistics.Commands.CreateLinkVisitStatistic;
public sealed class CreateLinkVisitStatisticValidator : AbstractValidator<CreateLinkVisitStatisticCommand>
{
    public CreateLinkVisitStatisticValidator()
    {
        RuleFor(x => x.LinkId).IsIdentifier();
    }
}

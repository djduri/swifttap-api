using FluentValidation;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Statistics.LinkVisitStatistics.Queries.GetSampledLinkVisitStatistics;
public sealed class GetSampledLinkVisitStatisticsValidator : AbstractValidator<GetSampledLinkVisitStatisticsQuery>
{
    public GetSampledLinkVisitStatisticsValidator()
    {
        RuleFor(x => x.LinkId).IsIdentifier();
        RuleFor(x => x.StartDate).NotEmpty().LessThanOrEqualTo(x => x.EndDate);
        RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(x => x.StartDate);
        RuleFor(x => x.NumberOfSamples).NotEmpty().GreaterThan(0).GreaterThan(0).LessThanOrEqualTo(500);
    }
}

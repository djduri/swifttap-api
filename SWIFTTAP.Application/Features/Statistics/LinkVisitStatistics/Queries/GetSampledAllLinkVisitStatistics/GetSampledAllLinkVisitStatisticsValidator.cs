using FluentValidation;

namespace SWIFTTAP.Application.Features.Statistics.LinkVisitStatistics.Queries.GetSampledAllLinkVisitStatistics;
public sealed class GetSampledAllLinkVisitStatisticsValidator : AbstractValidator<GetSampledAllLinkVisitStatisticsQuery>
{
    public GetSampledAllLinkVisitStatisticsValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty().LessThanOrEqualTo(x => x.EndDate);
        RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(x => x.StartDate);
        RuleFor(x => x.NumberOfSamples).NotEmpty().GreaterThan(0).GreaterThan(0).LessThanOrEqualTo(500);
    }
}

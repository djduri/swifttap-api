using FluentValidation;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitStatistics.Queries.GetSampledAllCardVisitStatistics;
public sealed class GetSampledAllCardVisitStatisticsValidator : AbstractValidator<GetSampledAllCardVisitStatisticsQuery>
{
    public GetSampledAllCardVisitStatisticsValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty().LessThanOrEqualTo(x => x.EndDate);
        RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(x => x.StartDate);
        RuleFor(x => x.NumberOfSamples).NotEmpty().GreaterThan(0).GreaterThan(0).LessThanOrEqualTo(500);
    }
}

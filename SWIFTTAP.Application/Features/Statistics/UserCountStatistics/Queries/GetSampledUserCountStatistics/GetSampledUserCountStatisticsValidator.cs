using FluentValidation;

namespace SWIFTTAP.Application.Features.Statistics.UserCountStatistics.Queries.GetSampledUserCountStatistics;
public sealed class GetSampledUserCountStatisticsValidator : AbstractValidator<GetSampledUserCountStatisticsQuery>
{
    public GetSampledUserCountStatisticsValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty().LessThanOrEqualTo(x => x.EndDate);
        RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(x => x.StartDate);
        RuleFor(x => x.NumberOfSamples).NotEmpty().GreaterThan(0).GreaterThan(0).LessThanOrEqualTo(500);
    }
}

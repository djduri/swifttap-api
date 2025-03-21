using FluentValidation;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitStatistics.Queries.GetSampledCardVisitStatistics;
public sealed class GetSampledCardVisitStatisticsValidator : AbstractValidator<GetSampledCardVisitStatisticsQuery>
{
    public GetSampledCardVisitStatisticsValidator()
    {
        RuleFor(x => x.CardId).IsIdentifier();
        RuleFor(x => x.StartDate).NotEmpty().LessThanOrEqualTo(x => x.EndDate);
        RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(x => x.StartDate);
        RuleFor(x => x.NumberOfSamples).NotEmpty().GreaterThan(0).GreaterThan(0).LessThanOrEqualTo(500);
    }
}

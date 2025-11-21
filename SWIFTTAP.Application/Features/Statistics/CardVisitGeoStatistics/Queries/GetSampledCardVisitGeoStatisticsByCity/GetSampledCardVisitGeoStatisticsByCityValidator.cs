using FluentValidation;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitGeoStatistics.Queries.GetSampledCardVisitGeoStatisticsByCity;

public sealed class GetSampledCardVisitGeoStatisticsByCityValidator : AbstractValidator<GetSampledCardVisitGeoStatisticsByCityQuery>
{
    public GetSampledCardVisitGeoStatisticsByCityValidator()
    {
        //RuleFor(x => x.CardId).IsIdentifier();
        RuleFor(x => x.StartDate).NotEmpty().LessThanOrEqualTo(x => x.EndDate);
        RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(x => x.StartDate);
        RuleFor(x => x.SampleType).IsInEnum();
    }
}

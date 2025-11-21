using FluentValidation;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitGeoStatistics.Queries.GetSampledCardVisitGeoStatisticsByCountry;
public sealed class GetSampledCardVisitGeoStatisticsByCountryValidator : AbstractValidator<GetSampledCardVisitGeoStatisticsByCountryQuery>
{
    public GetSampledCardVisitGeoStatisticsByCountryValidator()
    {
        //RuleFor(x => x.CardId).IsIdentifier();
        RuleFor(x => x.StartDate).NotEmpty().LessThanOrEqualTo(x => x.EndDate);
        RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(x => x.StartDate);
        RuleFor(x => x.SampleType).IsInEnum();
    }
}

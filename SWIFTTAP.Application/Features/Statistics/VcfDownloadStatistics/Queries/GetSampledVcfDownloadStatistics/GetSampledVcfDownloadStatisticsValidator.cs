using FluentValidation;

namespace SWIFTTAP.Application.Features.Statistics.VcfDownloadStatistics.Queries.GetSampledVcfDownloadStatistics;
public sealed class GetSampledVcfDownloadStatisticsValidator : AbstractValidator<GetSampledVcfDownloadStatisticsQuery>
{
    public GetSampledVcfDownloadStatisticsValidator()
    {
        RuleFor(x => x.UniqueName).NotEmpty();
        RuleFor(x => x.StartDate).NotEmpty().LessThanOrEqualTo(x => x.EndDate);
        RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(x => x.StartDate);
        RuleFor(x => x.NumberOfSamples).NotEmpty().GreaterThan(0).GreaterThan(0).LessThanOrEqualTo(500);
    }
}

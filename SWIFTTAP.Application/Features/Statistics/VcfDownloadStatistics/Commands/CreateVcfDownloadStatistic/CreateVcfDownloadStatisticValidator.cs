using FluentValidation;

namespace SWIFTTAP.Application.Features.Statistics.VcfDownloadStatistics.Commands.CreateVcfDownloadStatistic;
public sealed class CreateVcfDownloadStatisticValidator : AbstractValidator<CreateVcfDownloadStatisticCommand>
{
    public CreateVcfDownloadStatisticValidator()
    {
        RuleFor(x => x.UniqueName).NotEmpty();
    }
}

using FluentValidation;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitStatistics.Commands.CreateCardVisitStatistic;
public sealed class CreateCardVisitStatisticValidator : AbstractValidator<CreateCardVisitStatisticCommand>
{
    public CreateCardVisitStatisticValidator()
    {
        RuleFor(x => x.CardId).IsIdentifier();
    }
}

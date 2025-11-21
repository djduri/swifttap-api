using FluentValidation;
using SWIFTTAP.Application.Common;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitGeoStatistics.Commands.CreateCardVisitGeoStatistic
{
    public sealed class CreateCardVisitGeoStatisticValidator : AbstractValidator<CreateCardVisitGeoStatisticCommand>
    {
        public CreateCardVisitGeoStatisticValidator()
        {
            RuleFor(x => x.CardId).IsIdentifier();
        }
    }
}

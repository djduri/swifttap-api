using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Cards.Cards.Specifications;
using SWIFTTAP.Application.Features.Statistics.VcfDownloadStatistics.Specifications;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Domain.Statistics;
using SWIFTTAP.Infrastructure.Abstractions;
using System.Data;

namespace SWIFTTAP.Application.Features.Statistics.VcfDownloadStatistics.Commands.CreateVcfDownloadStatistic;
internal sealed class CreateVcfDownloadStatisticHandler : ICommandHandler<CreateVcfDownloadStatisticCommand, long>
{
    private readonly IRepository<VcfDownloadStatistic> _vcfDownloadStatisticRepository;
    private readonly IRepository<Card> _cardRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateVcfDownloadStatisticHandler> _logger;

    public CreateVcfDownloadStatisticHandler(IRepository<VcfDownloadStatistic> vcfDownloadStatisticrepository,
                                             IRepository<Card> cardRepository,
                                             IUnitOfWork unitOfWork,
                                             ILogger<CreateVcfDownloadStatisticHandler> logger)
    {
        _vcfDownloadStatisticRepository = vcfDownloadStatisticrepository;
        _unitOfWork = unitOfWork;
        _cardRepository = cardRepository;
        _logger = logger;
    }

    public async Task<long> Handle(CreateVcfDownloadStatisticCommand request, CancellationToken cancellationToken)
    {
        // Start a transaction with Serializable isolation level.
        await using var transaction = await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        try
        {
            var card = await _cardRepository.GetAsync(new FindCardByUniqueNameSpecification(request.UniqueName), cancellationToken);
            if (card is null)
            {
                _logger.LogWarning("Card with UniqueName {UniqueName} not found.", request.UniqueName);
                throw EntityNotFoundException.FromErrorCode(ErrorCodes.Card.NotFound);
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var vcdDownloadStatistic = await _vcfDownloadStatisticRepository
                .GetAsync(new FindVcdDownloadStatisticByCardIdAndDateSpecification(card.Id, today), cancellationToken);

            if (vcdDownloadStatistic is null)
            {
                vcdDownloadStatistic = VcfDownloadStatistic.Factory.Create(card.Id);
                _vcfDownloadStatisticRepository.Add(vcdDownloadStatistic);
            }

            vcdDownloadStatistic.IncrementCounter();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return vcdDownloadStatistic.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing vcw download statistic for UniqueName {UniqueName}", request.UniqueName);
            await transaction.RollbackAsync(cancellationToken);  // Rollback in case of error
            throw;
        }
    }

}
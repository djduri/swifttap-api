using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Statistics.CardVisitStatistics.Specifications;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Domain.Statistics;
using SWIFTTAP.Infrastructure.Abstractions;
using System.Data;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitStatistics.Commands.CreateCardVisitStatistic;
internal sealed class CreateCardVisitStatisticHandler : ICommandHandler<CreateCardVisitStatisticCommand, long>
{
    private readonly IRepository<CardVisitStatistic> _cardVisitStatRepository;
    private readonly IRepository<Card> _cardRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateCardVisitStatisticHandler> _logger;

    public CreateCardVisitStatisticHandler(IRepository<CardVisitStatistic> repository,
                                           IRepository<Card> cardRepository,
                                           IUnitOfWork unitOfWork,
                                           ILogger<CreateCardVisitStatisticHandler> logger)
    {
        _cardVisitStatRepository = repository;
        _unitOfWork = unitOfWork;
        _cardRepository = cardRepository;
        _logger = logger;
    }

    public async Task<long> Handle(CreateCardVisitStatisticCommand request, CancellationToken cancellationToken)
    {
        // Start a transaction with Serializable isolation level to prevent race conditions.
        await using var transaction = await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        try
        {
            if (!await _cardRepository.AnyAsync(request.CardId, cancellationToken))
            {
                _logger.LogWarning("Card with ID {CardId} not found.", request.CardId);
                throw EntityNotFoundException.FromErrorCode(ErrorCodes.Card.NotFound);
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var cardVisitStatistic = await _cardVisitStatRepository
                .GetAsync(new FindCardVisitStatisticByCardIdAndDateSpecification(request.CardId, today), cancellationToken);

            if (cardVisitStatistic is null)
            {
                cardVisitStatistic = CardVisitStatistic.Factory.Create(request.CardId);
                _cardVisitStatRepository.Add(cardVisitStatistic);
            }

            cardVisitStatistic.IncrementCounter();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return cardVisitStatistic.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing card visit statistics for Card ID {CardId}", request.CardId);
            await transaction.RollbackAsync(cancellationToken);  // Rollback in case of error
            throw;
        }
    }

}

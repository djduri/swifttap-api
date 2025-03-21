using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Features.Statistics.LinkVisitStatistics.Specifications;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Domain.Statistics;
using SWIFTTAP.Infrastructure.Abstractions;
using System.Data;

namespace SWIFTTAP.Application.Features.Statistics.LinkVisitStatistics.Commands.CreateLinkVisitStatistic;
internal sealed class CreateLinkVisitStatisticHandler : ICommandHandler<CreateLinkVisitStatisticCommand, long>
{
    private readonly IRepository<LinkVisitStatistic> _linkVisitStatRepository;
    private readonly IRepository<Link> _linkRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateLinkVisitStatisticHandler> _logger;

    public CreateLinkVisitStatisticHandler(IRepository<LinkVisitStatistic> repository,
                                           IRepository<Link> linkRepository,
                                           IUnitOfWork unitOfWork,
                                           ILogger<CreateLinkVisitStatisticHandler> logger)
    {
        _linkVisitStatRepository = repository;
        _unitOfWork = unitOfWork;
        _linkRepository = linkRepository;
        _logger = logger;
    }

    public async Task<long> Handle(CreateLinkVisitStatisticCommand request, CancellationToken cancellationToken)
    {
        // Start a transaction with Serializable isolation level.
        await using var transaction = await _unitOfWork.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        try
        {
            if (!await _linkRepository.AnyAsync(request.LinkId, cancellationToken))
            {
                _logger.LogWarning("Link with ID {LinkId} not found.", request.LinkId);
                throw EntityNotFoundException.FromErrorCode(ErrorCodes.Link.NotFound);
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var linkVisitStatistic = await _linkVisitStatRepository
                .GetAsync(new FindLinkVisitStatisticByLinkIdAndDateSpecification(request.LinkId, today), cancellationToken);

            if (linkVisitStatistic is null)
            {
                linkVisitStatistic = LinkVisitStatistic.Factory.Create(request.LinkId);
                _linkVisitStatRepository.Add(linkVisitStatistic);
            }

            linkVisitStatistic.IncrementCounter();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return linkVisitStatistic.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing link visit statistics for Link ID {LinkId}", request.LinkId);
            await transaction.RollbackAsync(cancellationToken);  // Rollback in case of error
            throw;
        }
    }

}
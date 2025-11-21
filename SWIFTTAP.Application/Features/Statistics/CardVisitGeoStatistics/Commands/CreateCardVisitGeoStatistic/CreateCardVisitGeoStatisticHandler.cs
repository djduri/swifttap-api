using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Services.Interfaces;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.Messages;
using SWIFTTAP.Domain.Statistics;
using SWIFTTAP.Infrastructure.Abstractions;
using System.Data;

namespace SWIFTTAP.Application.Features.Statistics.CardVisitGeoStatistics.Commands.CreateCardVisitGeoStatistic
{
    internal sealed class CreateCardVisitGeoStatisticHandler : ICommandHandler<CreateCardVisitGeoStatisticCommand, long>
    {
        private readonly IRepository<CardVisitGeoStatistic> _cardVisitGeoStatRepository;
        private readonly IRepository<Card> _cardRepository;
        private readonly ICurrentScopeService _currentScopeService;
        private readonly IIpGeolocationService _ipGeolocationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCardVisitGeoStatisticHandler> _logger;

        public CreateCardVisitGeoStatisticHandler(IRepository<CardVisitGeoStatistic> cardVisitGeoStatRepository,
                                                  IRepository<Card> cardRepository,
                                                  ICurrentScopeService currentScopeService,
                                                  IIpGeolocationService ipGeolocationService,
                                                  IUnitOfWork unitOfWork,
                                                  ILogger<CreateCardVisitGeoStatisticHandler> logger)
        {
            _cardVisitGeoStatRepository = cardVisitGeoStatRepository;
            _cardRepository = cardRepository;
            _currentScopeService = currentScopeService;
            _ipGeolocationService = ipGeolocationService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<long> Handle(CreateCardVisitGeoStatisticCommand request, CancellationToken cancellationToken)
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

                var ipAddress = _currentScopeService.GetIpAddress();

                if (string.IsNullOrEmpty(ipAddress))
                {
                    _logger.LogWarning("Cannot get IP Address.");
                    throw EntityCreateException.FromErrorCode(ErrorCodes.CardVisitGeoStatistic.IpAddressGetFailed);
                }

                var geolocation = await _ipGeolocationService.GetGeolocationAsync(ipAddress, cancellationToken);

                if (geolocation is null 
                    || string.IsNullOrEmpty(geolocation.City)
                    || string.IsNullOrEmpty(geolocation.CountryCode))
                {
                    _logger.LogWarning("Geolocation lookup failed for IP address {IpAddress}.", ipAddress);
                    throw EntityCreateException.FromErrorCode(ErrorCodes.CardVisitGeoStatistic.GeolocationLookupFailed);
                }

                var cardVisitGeoStatistic = CardVisitGeoStatistic.Factory.Create(request.CardId, geolocation.CountryCode, geolocation.City);
                _cardVisitGeoStatRepository.Add(cardVisitGeoStatistic);               

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return cardVisitGeoStatistic.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing card visit geo statistics for Card ID {CardId}", request.CardId);
                await transaction.RollbackAsync(cancellationToken);  // Rollback in case of error
                throw;
            }
        }
    }    
}
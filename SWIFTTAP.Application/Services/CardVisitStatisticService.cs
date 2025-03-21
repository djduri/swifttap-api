using SWIFTTAP.Domain.Statistics;
using SWIFTTAP.Infrastructure.Abstractions;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Services;
internal sealed class CardVisitStatisticService
{
    private readonly IRepository<CardVisitStatistic> _cardVisitStatisticRepository;
    private readonly DatabaseContext _dbContext;

    public CardVisitStatisticService(IRepository<CardVisitStatistic> cardVisitStatsticRepository, DatabaseContext dbContext)
    { 
        _cardVisitStatisticRepository = cardVisitStatsticRepository;
        _dbContext = dbContext;
    }

    public async Task IncrementCardVisitCounter(long cardId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);


    }
}

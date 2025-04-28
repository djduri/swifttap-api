using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Jobs.Interfaces;
using SWIFTTAP.Domain.Statistics;
using SWIFTTAP.Infrastructure.Database;
using System.Diagnostics;

namespace SWIFTTAP.Application.Jobs;
public sealed class UserCountStatisticBackgroundJob : IUserCountStatisticBackgroundJob
{
    private readonly DatabaseContext _dbContext;
    private readonly ILogger<UserCountStatisticBackgroundJob> _logger;

    public UserCountStatisticBackgroundJob(DatabaseContext dbContext, ILogger<UserCountStatisticBackgroundJob> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Run(CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();

        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var now = DateTime.UtcNow;

            var usersCount = await _dbContext.Users.CountAsync();

            var userCountStatistic = UserCountStatistic.Factory.Create(usersCount);

            await _dbContext.UserCountStatistics.AddAsync(userCountStatistic);

            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(CancellationToken.None);

            stopwatch.Stop();

            _logger.LogInformation("UserCountStatisticBackgroundJob finished successfully. {UsersCount} users already exists. Execution took {ExecutionTime}ms",
                                    usersCount,
                                    stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(CancellationToken.None);

            stopwatch.Stop();

            _logger.LogError("UserCountStatisticBackgroundJob failed with {Exception}. Execution took {ExecutionTime}ms",
                ex,
                stopwatch.ElapsedMilliseconds);
        }
    }
}

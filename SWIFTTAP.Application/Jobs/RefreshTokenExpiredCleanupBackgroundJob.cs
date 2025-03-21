using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using SWIFTTAP.Application.Jobs.Interfaces;
using SWIFTTAP.Infrastructure.Database;

namespace SWIFTTAP.Application.Jobs;
public sealed class RefreshTokenExpiredCleanupBackgroundJob : IRefreshTokenExpiredCleanupBackgroundJob
{
    private readonly DatabaseContext _dbContext;
    private readonly ILogger<RefreshTokenExpiredCleanupBackgroundJob> _logger;

    public RefreshTokenExpiredCleanupBackgroundJob(DatabaseContext dbContext, ILogger<RefreshTokenExpiredCleanupBackgroundJob> logger)
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

            var expiredRefreshTokens = await _dbContext.RefreshTokens
                                            .Where(x => x.RevokedAt != null || x.ExpiresAt < now)
                                            .ToListAsync(cancellationToken);

            _dbContext.RefreshTokens.RemoveRange(expiredRefreshTokens);

            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(CancellationToken.None);

            stopwatch.Stop();

            _logger.LogInformation("RefreshTokenExpiredCleanupBackgroundJob finished successfully. {ExpiredRefreshTokensCount} expired refresh tokens were removed. Execution took {ExecutionTime}ms",
                                    expiredRefreshTokens.Count,
                                    stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(CancellationToken.None);

            stopwatch.Stop();

            _logger.LogError("RefreshTokenExpiredCleanupBackgroundJob failed with {Exception}. Execution took {ExecutionTime}ms",
                ex,
                stopwatch.ElapsedMilliseconds);
        }
    }
}

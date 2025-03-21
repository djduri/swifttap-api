using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Infrastructure.Database;

internal sealed class Transaction : ITransaction
{
    private readonly IDbContextTransaction _transaction;
    private bool _disposed;

    private Transaction(IDbContextTransaction transaction)
    {
        _transaction = transaction;
        _disposed = false;
    }

    public void Commit()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(Transaction));

        _transaction.Commit();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(Transaction));

        await _transaction.CommitAsync(cancellationToken);
    }

    public void Rollback()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(Transaction));

        _transaction.Rollback();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(Transaction));

        await _transaction.RollbackAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction.Dispose();
            }

            _disposed = true;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            if (_transaction is not null)
            {
                await _transaction.DisposeAsync();
            }

            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }

    public static Transaction Create(DatabaseContext databaseContext)
    {
        return new Transaction(databaseContext.Database.BeginTransaction());
    }

    public static async Task<Transaction> CreateAsync(DatabaseContext databaseContext)
    {
        return new Transaction(await databaseContext.Database.BeginTransactionAsync(CancellationToken.None));
    }

    public static async Task<Transaction> CreateAsync(DatabaseContext databaseContext,
                                                      System.Data.IsolationLevel isolationLevel,
                                                      CancellationToken cancellationToken = default)
    {
        var dbTransaction = await databaseContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        return new Transaction(dbTransaction);
    }

}

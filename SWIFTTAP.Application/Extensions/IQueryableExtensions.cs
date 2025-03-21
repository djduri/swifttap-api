using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Application.Common;
using SWIFTTAP.Domain.Core;
using System.Linq.Expressions;

namespace SWIFTTAP.Application.Extensions;

public static class IQueryableExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> @this,
        PaginationArguments? paginationArguments,
        CancellationToken cancellationToken = default)
    {
        if (paginationArguments is null)
            return await @this.ToPagedListAsync(cancellationToken);

        return await @this.ToPagedListAsync(
            paginationArguments.StartNumber,
            paginationArguments.NumberOfRecords,
            cancellationToken);
    }

    public static async Task<PagedList<TSource>> ToPagedListAsync<TSource>(this IQueryable<TSource> @this, CancellationToken cancellationToken = default)
    {
        var all = await @this.ToListAsync(cancellationToken);

        return new PagedList<TSource>(all, all.Count);
    }
    
    public static async Task<PagedList<TSource>> ToPagedListAsync<TSource>(this IQueryable<TSource> @this, int skip, int take, CancellationToken cancellationToken = default)
    {
        var total = await @this.CountAsync(cancellationToken);
        var paged = await @this.Skip(skip).Take(take).ToListAsync(cancellationToken);

        return new PagedList<TSource>(paged, total);
    }
   
    public static ISortExpressionBuilder<TSource> Sort<TSource>(this IQueryable<TSource> @this, string sortBy, bool descending = false)
    {
        return new SortExpressionBuilder<TSource>(@this)
            .By(sortBy, descending);
    }
  
    public static IQueryable<T> Filter<T>(this IQueryable<T> @this, Expression<Func<T, string>> memberExpression, string value)
    {
        ArgumentNullException.ThrowIfNull(memberExpression);
        ArgumentNullException.ThrowIfNull(value);

        var parameter = memberExpression.Parameters.FirstOrDefault() ??
            throw new InvalidOperationException("Can not access member expression parameter.");

        Expression<Func<string, bool>> filterExpression = x => EF.Functions.ILike(x, $"%{value}%");

        var whereExpressionBody = Expression.Invoke(filterExpression, memberExpression.Body);

        var whereExpression = Expression.Lambda<Func<T, bool>>(whereExpressionBody, parameter);

        return @this.Where(whereExpression);
    }
}

using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Domain.Base;
using SWIFTTAP.Infrastructure.Abstractions;

namespace SWIFTTAP.Infrastructure.Database;

internal static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity>(IQueryable<TEntity> collection, ISpecification<TEntity> specification)
        where TEntity : class, IEntity
    {
        IQueryable<TEntity> queryable = collection;

        if (specification.Criteria is not null)
        {
            queryable = queryable.Where(specification.Criteria);
        }

        queryable = specification.IncludeExpressions.Aggregate(queryable, (current, include) => current.Include(include));

        if (specification.OrderByExpression is not null)
        {
            queryable = queryable.OrderBy(specification.OrderByExpression);
        }
        else if (specification.OrderByDescendingExpression is not null)
        {
            queryable = queryable.OrderByDescending(specification.OrderByDescendingExpression);
        }

        return queryable;
    }
}

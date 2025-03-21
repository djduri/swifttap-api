using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace SWIFTTAP.Infrastructure.Extensions;

public static class DbContextExtensions
{
	public static IQueryable<T> Include<T, TProperty>(this EntityEntry<T> entry, Expression<Func<T, TProperty>> navigationProperty)
		where T : class
	{
		var dbSet = entry.Context.Set<T>();
		return dbSet.Include(navigationProperty);
	}
}

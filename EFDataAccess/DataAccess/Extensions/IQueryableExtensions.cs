using Common.Pagination;
using Common.ValueTypes;
using FlatOffersTracker.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace EFRepository.DataAccess.Extensions
{
	public static class IQueryableExtensions
	{
		public static IQueryable<FlatOffer> WhereDateWithinRange<T>(this IQueryable<FlatOffer> query, 
			Expression<Func<FlatOffer, T>> fieldExp, 
			DateRange range)
		{
			var memExp = fieldExp.Body as MemberExpression;
			var pexp = Expression.Parameter(typeof(FlatOffer));
			var propertyExp = Expression.Property(pexp, memExp.Member.Name);
			var endDateExp = range.EndDate.HasValue ? Expression.LessThanOrEqual(propertyExp, ValueExpression(range.EndDate, memExp)) : null;
			var startDateExp = range.StartDate.HasValue ? Expression.GreaterThanOrEqual(propertyExp, ValueExpression(range.StartDate, memExp)) : null;

			if (range.EndDate.HasValue && range.StartDate.HasValue)
			{
				return query.Where(GetExpression(Expression.AndAlso(startDateExp, endDateExp), pexp));
			}

			if (range.StartDate.HasValue)
			{
				return query.Where(GetExpression(startDateExp, pexp));
			}

			return query.Where(GetExpression(endDateExp, pexp));
		}

		public static PaginatedResult<T> ToPaginated<T>(this IQueryable<T> query, QueryPagination pagination)
		{
			var toSkip = (pagination.Page - 1) * pagination.PageSize;

			var res = query.Select(x => new { Count = query.Count(), Entity = x })
				.Skip(toSkip)
				.Take(pagination.PageSize)
				.ToList();

			return new PaginatedResult<T>(
					res.Any() ? res.First().Count : 0,
					pagination.Page,
					pagination.PageSize,
					res.Select(x => x.Entity).ToList()
				);
		}

		private static UnaryExpression ValueExpression(DateTime? date, MemberExpression memExp)
		{
			return Expression.Convert(Expression.Constant(date.Value), memExp.Type);
		}

		public static Expression<Func<FlatOffer, bool>> GetExpression(BinaryExpression exp, ParameterExpression pexp)
		{
			return Expression.Lambda<Func<FlatOffer, bool>>(exp, pexp);
		}
	}
}
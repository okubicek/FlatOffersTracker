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
using Common.ValueTypes;
using FlatOffersTracker.Entities;
using System.Linq;

namespace EFRepository.DataAccess.Extensions
{
	public static class IQueryableExtensions
	{
		public static IQueryable<FlatOffer> WhereDateAddedWithinRange(this IQueryable<FlatOffer> query, DateRange range)
		{
			if (range.EndDate.HasValue && range.StartDate.HasValue)
			{
				return query.Where(x => x.DateAdded >= range.StartDate && x.DateAdded <= range.EndDate);
			}

			if (range.StartDate.HasValue)
			{
				return query.Where(x => x.DateAdded >= range.StartDate);
			}

			return query.Where(x => x.DateAdded <= range.EndDate);
		}

		public static IQueryable<FlatOffer> WhereDateRemovedWithinRange(this IQueryable<FlatOffer> query, DateRange range)
		{
			if (range.EndDate.HasValue && range.StartDate.HasValue)
			{
				return query.Where(x => x.DateRemoved >= range.StartDate && x.DateRemoved <= range.EndDate);
			}

			if (range.StartDate.HasValue)
			{
				return query.Where(x => x.DateRemoved >= range.StartDate);
			}

			return query.Where(x => x.DateRemoved <= range.EndDate);
		}
	}
}
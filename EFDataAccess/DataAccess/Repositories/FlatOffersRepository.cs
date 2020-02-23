using EFRepository.DataAccess.Context;
using FlatOffersTracker.Entities;
using System.Collections.Generic;
using System.Linq;
using FlatOffersTracker.DataAccess;
using Microsoft.EntityFrameworkCore;
using Common.ValueTypes;
using EFRepository.DataAccess.Extensions;
using Common.Pagination;

namespace EFRepository.DataAccess.Repositories
{
	public class FlatOffersRepository : IFlatOffersRepository
	{
		private FlatOffersDbContext _dbContext;

		public FlatOffersRepository(FlatOffersDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IEnumerable<FlatOffer> GetOpenFlatOffers()
		{
			return _dbContext.FlatOffers
				.Include(x => x.Links)
				.Include(x => x.Notifications)
				.Where(x => x.DateRemoved == null);
		}

		public PaginatedResult<FlatOffer> Get(
			FlatType? flatType, 
			int? minFlatSize,
			int? numberOfRooms,
			decimal? maxPrice,
			DateRange? dateAdded,
			DateRange? dateRemoved,
			QueryPagination pagination)
		{
			var query = _dbContext.FlatOffers.AsQueryable();

			if (flatType.HasValue)
			{
				query = query.Where(x => x.FlatType == flatType.Value);
			}

			if (minFlatSize.HasValue)
			{
				query = query.Where(x => x.FlatSize >= minFlatSize.Value);
			}

			if (numberOfRooms.HasValue)
			{
				query = query.Where(x => x.NumberOfRooms == numberOfRooms.Value);
			}

			if (maxPrice.HasValue)
			{
				query = query.Where(x => x.Price <= maxPrice.Value);
			}

			if (dateAdded.HasValue)
			{
				query = query.WhereDateWithinRange(x => x.DateAdded, dateAdded.Value);
			}

			if (dateRemoved.HasValue)
			{
				query = query.WhereDateWithinRange(x => x.DateRemoved, dateRemoved.Value);
			}

			return query
				.Include(x => x.Links)
				.Include(x => x.Notifications)
				.Where(x => x.DateRemoved == null || x.Notifications.Any(y => !y.Viewed))
				.ToPaginated(pagination);
		}

		public void Save(IEnumerable<FlatOffer> offers)
		{
			_dbContext.FixStateOfEntitiesWithoutKey(offers.SelectMany(x => x.Notifications));
			_dbContext.FixStateOfEntitiesWithoutKey(offers.SelectMany(x => x.Links));
			_dbContext.FixStateOfEntitiesWithoutKey(offers.Where(x => x.Images != null).SelectMany(x => x.Images));

			_dbContext.FlatOffers.UpdateRange(offers);
			_dbContext.SaveChanges();
		}

		public HashSet<int> GetFlatOfferIdsWithStoredImages()
		{
			return _dbContext
				.FlatOffers
				.Where(x => !x.Removed)
				.Select(x => new { FlatOfferId = x.Id, Count = x.Images.Count() })
				.Where(x => x.Count > 0)
				.Select(x => x.FlatOfferId.Value)
				.Distinct()
				.ToHashSet();
		}

		public IEnumerable<byte[]> GetImages(int flatOfferId, bool headOnly)
		{
			var query = _dbContext
				.FlatOffers
				.Where(x => x.Id == flatOfferId)
				.SelectMany(x => x.Images);

			if (headOnly)
			{
				query = query.Where(x => x.SortOrder == 1);
			}

			return query
				.Select(x => x.Content)
				.ToList();
		}
	}
}

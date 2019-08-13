using EFRepository.DataAccess.Context;
using FlatOffersTracker.Entities;
using System.Collections.Generic;
using System.Linq;
using FlatOffersTracker.DataAccess;
using Microsoft.EntityFrameworkCore;
using Common.ValueTypes;
using EFRepository.DataAccess.Extensions;

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

		public IEnumerable<FlatOffer> Get(
			FlatType? flatType, 
			int? minFlatSize, 
			int? numberOfRooms, 
			decimal? maxPrice, 
			DateRange? dateAdded, 
			DateRange? dateRemoved)
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
				query = query.WhereDateAddedWithinRange(dateAdded.Value);
			}

			if (dateRemoved.HasValue)
			{
				query = query.WhereDateRemovedWithinRange(dateRemoved.Value);
			}

			return query.Include(x => x.Links).ToList();
		}

		public void Save(IEnumerable<FlatOffer> offers)
		{
			_dbContext.FixStateOfEntitiesWithoutKey(offers.SelectMany(x => x.Notifications));
			_dbContext.FixStateOfEntitiesWithoutKey(offers.SelectMany(x => x.Links));

			_dbContext.FlatOffers.UpdateRange(offers);
			_dbContext.SaveChanges();
		}
	}
}

using EFRepository.DataAccess.Context;
using FlatOffersTracker.Entities;
using System.Collections.Generic;
using System.Linq;
using FlatOffersTracker.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;

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

		public IEnumerable<FlatOffer> Get()
		{
			throw new NotImplementedException();
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

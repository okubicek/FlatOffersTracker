using FlatOffersTrackerBackgroundApp.DataAccess.Context;
using FlatOffersTracker.Entities;
using System.Collections.Generic;
using System.Linq;
using FlatOffersTracker.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace FlatOffersTrackerBackgroundApp.DataAccess.Repositories
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

		public void Save(IEnumerable<FlatOffer> offers)
		{
			_dbContext.FlatOffers.UpdateRange(offers);
			_dbContext.SaveChanges();
		}
	}
}

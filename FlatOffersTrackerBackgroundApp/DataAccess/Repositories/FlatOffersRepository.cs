using FlatOffersTrackerBackgroundApp.DataAccess.Context;
using FlatOffersTracker.Entities;
using System.Collections.Generic;
using System.Linq;
using FlatOffersTracker.DataAccess;

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
				.Where(x => x.DateRemoved == null);
		}

		public void Save(IEnumerable<FlatOffer> offers)
		{
			_dbContext.Add(offers);
			_dbContext.SaveChanges();
		}
	}
}

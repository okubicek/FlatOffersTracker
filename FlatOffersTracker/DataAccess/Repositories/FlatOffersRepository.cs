using FlatOffersTracker.DataAccess.Context;
using FlatOffersTracker.Entities;
using System.Collections.Generic;
using System.Linq;

namespace FlatOffersTracker.DataAccess.Repositories
{
	public interface IFlatOffersRepository
	{
		IEnumerable<FlatOffer> GetOpenFlatOffers();

		void Save(IEnumerable<FlatOffer> offers);
	}

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

using FlatOffersTracker.Entities;
using System.Collections.Generic;

namespace FlatOffersTracker.DataAccess
{
	public interface IFlatOffersRepository
	{
		IEnumerable<FlatOffer> GetOpenFlatOffers();

		void Save(IEnumerable<FlatOffer> offers);
	}
}

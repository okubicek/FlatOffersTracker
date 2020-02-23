using Common.Pagination;
using Common.ValueTypes;
using FlatOffersTracker.Entities;
using System.Collections.Generic;

namespace FlatOffersTracker.DataAccess
{
	public interface IFlatOffersRepository
	{
		IEnumerable<FlatOffer> GetOpenFlatOffers();

		PaginatedResult<FlatOffer> Get(
			FlatType? flatType,
			int? minFlatSize,
			int? numberOfRooms,
			decimal? maxPrice,
			DateRange? dateAdded,
			DateRange? dateRemoved,
			QueryPagination pagination);

		HashSet<int> GetFlatOfferIdsWithStoredImages();

		void Save(IEnumerable<FlatOffer> offers);

		IEnumerable<byte[]> GetImages(int flatOfferId, bool headOnly);
	}
}

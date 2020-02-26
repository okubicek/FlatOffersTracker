using Common.Cqrs;
using FlatOffersTracker.DataAccess;
using FlatOffersTracker.Entities;

namespace FlatOffersTracker.Cqrs.Queries
{
	public interface IGetImagesPerOfferHandler : IQuery<FlatOffersWithImage>
	{
	}

	public class GetImagesPerOfferHandler : IGetImagesPerOfferHandler
	{
		IFlatOffersRepository _repo;

		public GetImagesPerOfferHandler(IFlatOffersRepository repo)
		{
			_repo = repo;
		}

		public FlatOffersWithImage Get()
		{
			return new FlatOffersWithImage(_repo.GetFlatOfferIdsWithStoredImages());
		}
	}
}

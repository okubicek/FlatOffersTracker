using Common.Cqrs;
using FlatOffersTracker.DataAccess;
using FlatOffersTracker.Entities;
using System.Collections.Generic;

namespace FlatOffersTracker.Cqrs.Queries
{
	public class GetImagesPerOfferHandler : IQuery<FlatOffersWithImage>
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

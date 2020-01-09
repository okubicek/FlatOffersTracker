using Common.Cqrs;
using FlatOffersTracker.DataAccess;
using System.Collections.Generic;

namespace FlatOffersTracker.Cqrs.Queries
{
	public class GetFlatOfferImagesHandler : IQuery<IEnumerable<byte[]>, GetFlatOfferImagesQuery>
	{
		private IFlatOffersRepository _repository;

		public GetFlatOfferImagesHandler(IFlatOffersRepository repository)
		{
			_repository = repository;
		}

		public IEnumerable<byte[]> Get(GetFlatOfferImagesQuery query)
		{
			return _repository.GetImages(query.FlatOfferId, query.ReturnHeadImageOnly);
		}
	}
}

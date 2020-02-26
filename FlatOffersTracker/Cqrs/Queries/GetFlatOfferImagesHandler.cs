using Common.Cqrs;
using FlatOffersTracker.DataAccess;
using System.Collections.Generic;

namespace FlatOffersTracker.Cqrs.Queries
{
	public interface IGetFlatOfferImagesHandler : IQuery<IEnumerable<byte[]>, GetFlatOfferImagesQuery>
	{
	}

	public class GetFlatOfferImagesHandler : IGetFlatOfferImagesHandler
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

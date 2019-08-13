using Common.Cqrs;
using FlatOffersTracker.DataAccess;
using FlatOffersTracker.Entities;
using System.Collections.Generic;

namespace FlatOffersTracker.Cqrs.Queries
{
	public class GetFlatOffersQueryHandler : IQuery<IEnumerable<FlatOffer>, GetFlatOffersQuery>
	{
		private IFlatOffersRepository _repository;

		public GetFlatOffersQueryHandler(IFlatOffersRepository repository)
		{
			_repository = repository;
		}

		public IEnumerable<FlatOffer> Get(GetFlatOffersQuery query)
		{
			return _repository.Get(
				query.FlatType,
				query.MinFlatSize,
				query.NumberOfRooms, 
				query.MaxPrice,
				query.DateAdded,
				query.DateRemoved
			);
		}
	}
}

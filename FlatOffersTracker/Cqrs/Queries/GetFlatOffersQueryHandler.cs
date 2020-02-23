using Common.Cqrs;
using Common.Pagination;
using FlatOffersTracker.DataAccess;
using FlatOffersTracker.Entities;

namespace FlatOffersTracker.Cqrs.Queries
{
	public class GetFlatOffersQueryHandler : IQuery<PaginatedResult<FlatOffer>, GetFlatOffersQuery>
	{
		private IFlatOffersRepository _repository;

		public GetFlatOffersQueryHandler(IFlatOffersRepository repository)
		{
			_repository = repository;
		}

		public PaginatedResult<FlatOffer> Get(GetFlatOffersQuery query)
		{
			return _repository.Get(
				query.FlatType,
				query.MinFlatSize,
				query.NumberOfRooms,
				query.MaxPrice,
				query.DateAdded,
				query.DateRemoved,
				query.Pagination
			);
		}
	}
}
using FlatOffersTracker.Entities;
using FlatOffersTracker.Entities.ValueTypes;

namespace FlatOffersTracker.Cqrs.Queries
{
	public class GetFlatOffersQuery
	{
		public FlatType? FlatType { get; set; }

		public int? MinFlatSize { get; set; }

		public int? NumberOfRooms { get; set; }

		public decimal? MaxPrice { get; set; }

		public DateRange? DateAdded { get; set; }

		public DateRange? DateRemoved { get; set; }
	}
}

using FlatOffersTracker.Entities;

namespace FlatOffersTracker.Parsing
{
	public class Query
	{
		public int NumberOfRooms { get; set; }

		public FlatType FlatType { get; set; }

		public decimal? PriceTopLimit { get; set; }

		public string Location { get; set; }
	}
}

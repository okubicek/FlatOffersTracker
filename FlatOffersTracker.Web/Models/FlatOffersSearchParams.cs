namespace FlatOffersTracker.Web.Models
{
	public class FlatOffersSearchParams
	{
		public int? MinFlatSize { get; set; }

		public int? RoomCount { get; set; }

		public int? MaxPrice { get; set; }

		public string FlatType { get; set; }
	}
}

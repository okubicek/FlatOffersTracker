using System;

namespace FlatOffersTracker.Web.Models
{
	public class FlatOffersSearchParams
	{
		public int? MinFlatSize { get; set; }

		public int? RoomCount { get; set; }

		public int? MaxPrice { get; set; }

		public string FlatType { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public int PageSize { get; set; }

		public int PageNumber { get; set; }
	}
}
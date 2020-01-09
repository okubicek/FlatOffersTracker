namespace FlatOffersTracker.Web.Models
{
	public class FlatOffer
	{
		public int Id { get; set; }

		public string Address { get; set; }

		public int FlatSize { get; set; }

		public int NumberOfRooms { get; set; }

		public string FlatType { get; set; }

		public string Url { get; set; }

		public decimal Price { get; set; }
	}
}

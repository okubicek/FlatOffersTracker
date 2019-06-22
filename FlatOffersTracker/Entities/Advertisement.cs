namespace FlatOffersTracker.Entities
{
	public class Advertisement
	{
		public string Url { get; set; }

		public string Address { get; set; }

		public string FlatSize { get; set; }

		public FlatType FlatType { get; set; }

		public int NumberOfRooms { get; set; }

		public decimal Price { get; set; }
	}
}

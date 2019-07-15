namespace FlatOffersTracker.Entities
{
	public class Link
	{
		private Link()
		{
		}

		public Link(string url, int uniqueId, FlatOffer offer)
		{
			FlatOffer = offer;
			Url = url;
			UniqueId = uniqueId;
		}

		public FlatOffer FlatOffer { get; set; }

		public string Url { get; set; }

		public int UniqueId { get; set; }
	}
}

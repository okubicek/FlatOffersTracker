namespace FlatOffersTracker.Entities
{
	public class Link
	{
		private Link()
		{
		}

		public Link(string url, FlatOffer offer)
		{
			FlatOffer = offer;
			Url = url;
		}

		public FlatOffer FlatOffer { get; set; }

		public string Url { get; set; }
	}
}

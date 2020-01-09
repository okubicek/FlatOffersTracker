namespace FlatOffersTracker.Cqrs.Queries
{
	public class GetFlatOfferImagesQuery
	{
		public int FlatOfferId { get; set; }

		public bool ReturnHeadImageOnly { get; set; }
	}
}
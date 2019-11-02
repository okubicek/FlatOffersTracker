namespace FlatOffersTracker.Entities
{
	public class Image
	{
		private Image()
		{
		}

		public Image(byte[] content, short sortOrder, FlatOffer flatOffer)
		{
			FlatOffer = flatOffer;
			Content = content;
			SortOrder = sortOrder;
		}

		public int? Id { get; set; }

		public FlatOffer FlatOffer { get; set; }

		public byte[] Content { get; set; }

		public short SortOrder { get; set; }
	}
}

namespace FlatOffersTracker.Entities
{
	public class Notification
	{
		public FlatOffer FlatOffer { get; set; }

		public NotificationType Type { get; set; }

		public bool Viewed { get; set; }
	}
}

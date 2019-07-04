namespace FlatOffersTracker.Entities
{
	public class Notification
	{
		private Notification()
		{
		}

		public Notification(FlatOffer flatOffer, NotificationType type)
		{
			FlatOffer = flatOffer;
			Type = type;
		}

		public FlatOffer FlatOffer { get; set; }

		public NotificationType Type { get; set; }

		public bool Viewed { get; private set; }

		public void MarkAsViewed()
		{
			Viewed = true;
		}
	}
}

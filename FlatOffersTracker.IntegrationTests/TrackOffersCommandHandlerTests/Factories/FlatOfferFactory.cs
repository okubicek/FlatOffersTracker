using FlatOffersTracker.Entities;
using System.Collections.Generic;

namespace FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests
{
	public static class FlatOfferFactory
	{
		public static FlatOffer GetFlatOfferType1()
		{
			var offer = new FlatOffer
			{
				Address = "Ble",
				FlatSize = 80,
				FlatType = FlatType.Brick,
				NumberOfRooms = 3,
				Price = 800000
			};
			offer.Links = new List<Link> { new Link("http://ble.com", 496846, offer) };
			offer.Notifications = new List<Notification> { new Notification(offer, NotificationType.OfferAdded) };
			offer.Images = new List<Image> { new Image(new byte[] { 0x25, 0x16 }, 1, offer) };
			return offer;
		}
	}
}
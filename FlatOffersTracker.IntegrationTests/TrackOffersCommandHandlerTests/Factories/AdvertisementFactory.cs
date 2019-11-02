using FlatOffersTracker.Entities;
using System.Linq;

namespace FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests
{
	public static class AdvertisementFactory
	{
		public static Advertisement GetAdverstisementType1()
		{
			return new Advertisement
			{
				Address = "Random Addres 123",
				FlatSize = 90,
				FlatType = FlatType.Panel,
				NumberOfRooms = 2,
				Price = 100000,
				UniqueId = 808080,
				Url = "https://Traaadlfsl.com",
				ImagesUrl = new System.Collections.Generic.List<string> { "www.something.jpg" }
			};
		}

		public static Advertisement GetAdvertisementBasedOnOffer(FlatOffer offer)
		{
			return new Advertisement
			{
				Address = offer.Address,
				FlatSize = offer.FlatSize,
				FlatType = offer.FlatType,
				NumberOfRooms = offer.NumberOfRooms,
				Price = offer.Price,
				UniqueId = offer.Links.First().UniqueId,
				Url = offer.Links.First().Url,
				ImagesUrl = new System.Collections.Generic.List<string> { "www.FakeUrl.com" }
			};
		}
	}
}

using Common.Cqrs;
using FlatOffersTracker.Cqrs.Queries;
using FlatOffersTracker.Entities;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FlatOffersTracker.Tests
{	
	public abstract class UpdateOffersBasedOnAdvertisementsTestsBase
	{
		protected abstract IEnumerable<FlatOffer> FlatOffers { get; }

		protected abstract IEnumerable<Advertisement> Advertisements { get; }

		protected IEnumerable<FlatOffer> Result { get; private set; }

		public UpdateOffersBasedOnAdvertisementsTestsBase()
		{
			SetupTestObjects();

			var flatOffersWithImageMock = new Mock<IQuery<FlatOffersWithImage>>();
			flatOffersWithImageMock.Setup(x => x.Get()).Returns(new FlatOffersWithImage(new HashSet<int> { 1, 2 }));

			var imageDownloaderMock = new Mock<IQuery<Dictionary<long, List<byte[]>>, GetImagesByUrlQuery>>();
			imageDownloaderMock.Setup(x => x.Get(It.IsAny<GetImagesByUrlQuery>())).Returns(new Dictionary<long, List<byte[]>>());

			var _update = new UpdateOffersBasedOnAdvertisementsCommandHandler(imageDownloaderMock.Object, flatOffersWithImageMock.Object);

			Result = _update.Execute(new UpdateOffersBasedOnAdvertisementsCommand
			{
				Advertisements = Advertisements,
				Offers = FlatOffers
			});
		}

		private void SetupTestObjects()
		{
			FirstAdvertisement = new Advertisement
			{
				Address = "Brno",
				Url = "http:bleblablue.cz",
				UniqueId = 123485,
				FlatSize = 80,
				FlatType = FlatType.Brick,
				NumberOfRooms = 2,
				Price = 1000
			};

			SecondAdvertisement = new Advertisement
			{
				Url = "http:bleblablue/Nadvori.cz",
				UniqueId = 14894566,
				Address = "Brno",
				FlatSize = 75,
				FlatType = FlatType.Panel,
				NumberOfRooms = 3,
				Price = 5000
			};
		}

		protected FlatOffer GenerateOfferFromAdvertisement(int id, Advertisement ad, decimal? priceOverride = null)
		{
			var offer = new FlatOffer
			{
				Id = id,
				Address = ad.Address,
				FlatSize = ad.FlatSize,
				FlatType = ad.FlatType,
				NumberOfRooms = ad.NumberOfRooms,
				Price = priceOverride.HasValue ? priceOverride.Value : ad.Price
			};

			offer.AddLink(ad.Url, ad.UniqueId);

			return offer;
		}

		protected Advertisement FirstAdvertisement;

		protected Advertisement SecondAdvertisement;
	}

	public class FirstLoadWhenNoFlatOffersExist : UpdateOffersBasedOnAdvertisementsTestsBase
	{
		protected override IEnumerable<FlatOffer> FlatOffers => new List<FlatOffer>();

		protected override IEnumerable<Advertisement> Advertisements => new List<Advertisement>
		{
			FirstAdvertisement,
			SecondAdvertisement
		};

		[Fact]
		public void AllAdvertisementsShouldBeConvertedToFlatOffers()
		{
			Assert.Equal(2, Result.Count());
		}

		[Fact]
		public void FlatOffersShouldHaveExpectedValues()
		{			
			AssertAdvertisementMatchesFlatOffer(FirstAdvertisement);

			AssertAdvertisementMatchesFlatOffer(SecondAdvertisement);
		}

		[Fact]
		public void BothFlatOffersShouldHaveOfferAddedEventRaised()
		{
			Assert.False(Result.Where(x => !x.Notifications.Any(y => y.Type == NotificationType.OfferAdded)).Any());
		}

		private void AssertAdvertisementMatchesFlatOffer(Advertisement toCompare)
		{
			Assert.NotNull(Result.FirstOrDefault(x =>
							x.Address.Equals(toCompare.Address) &&
							x.FlatSize == toCompare.FlatSize &&
							x.FlatType == toCompare.FlatType &&
							x.NumberOfRooms == toCompare.NumberOfRooms &&
							x.Price == toCompare.Price &&
							x.Links.Any(l => l.UniqueId == toCompare.UniqueId) &&
							x.Links.Any(l => l.Url.Equals(toCompare.Url))
						));
		}
	}

	public class WhenOffersAlreadyExistNothingShouldHappen : UpdateOffersBasedOnAdvertisementsTestsBase
	{
		protected override IEnumerable<FlatOffer> FlatOffers => new List<FlatOffer>
		{
			GenerateOfferFromAdvertisement(1, FirstAdvertisement),
			GenerateOfferFromAdvertisement(2, SecondAdvertisement),
		};

		protected override IEnumerable<Advertisement> Advertisements => new List<Advertisement>
		{
			FirstAdvertisement,
			SecondAdvertisement
		};

		[Fact]
		public void NoFlatOffersWereAdded()
		{
			Assert.Equal(2, Result.Count());
		}
	}

	public class WhenNewAdvertisementIsAddedToExistingOnes : UpdateOffersBasedOnAdvertisementsTestsBase
	{
		protected override IEnumerable<FlatOffer> FlatOffers => new List<FlatOffer>
		{
			GenerateOfferFromAdvertisement(1, FirstAdvertisement)
		};

		protected override IEnumerable<Advertisement> Advertisements => new List<Advertisement>
		{
			SecondAdvertisement
		};

		[Fact]
		public void NewFlatOfferShouldBeAdded()
		{
			Assert.Equal(2, Result.Count());
		}
	}

	public class WhenAdvertisementIsRemoved : UpdateOffersBasedOnAdvertisementsTestsBase
	{
		protected override IEnumerable<FlatOffer> FlatOffers => new List<FlatOffer>
		{
			GenerateOfferFromAdvertisement(1, FirstAdvertisement),
			GenerateOfferFromAdvertisement(2, SecondAdvertisement),
		};

		protected override IEnumerable<Advertisement> Advertisements => new List<Advertisement>
		{
			SecondAdvertisement
		};

		[Fact]
		public void OfferShouldBeMarkedAsRemoved()
		{
			Result.Single(x => x.DateRemoved.HasValue);
		}

		[Fact]
		public void OfferShouldRaiseNewNotification()
		{
			var offer = Result.Single(x => x.DateRemoved.HasValue);
			Assert.Contains(offer.Notifications, x => x.Type == NotificationType.OfferRemoved);
		}
	}

	public class WhenAmendingPriceOnAdvertisement : UpdateOffersBasedOnAdvertisementsTestsBase
	{
		protected override IEnumerable<FlatOffer> FlatOffers => new List<FlatOffer>
		{
			GenerateOfferFromAdvertisement(1, FirstAdvertisement, FirstAdvertisement.Price + 1500)			
		};

		protected override IEnumerable<Advertisement> Advertisements => new List<Advertisement> { FirstAdvertisement };

		[Fact]
		public void PriceOnOfferShouldBeChanged()
		{
			Assert.Equal(Result.First().Price, FirstAdvertisement.Price);
		}

		[Fact]
		public void PriceChangeNotificationShouldBeRaised()
		{
			var offer = Result.First();
			Assert.Contains(offer.Notifications, x => x.Type == NotificationType.PriceChanged);
		}
	}

}

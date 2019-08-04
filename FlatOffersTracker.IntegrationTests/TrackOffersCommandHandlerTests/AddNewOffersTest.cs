using FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests.Fixtures;
using EFRepository.DataAccess.Context;
using System.Linq;
using Xunit;

namespace FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests
{
	[Collection(FlatOffersDbContextFixtureDefinition.Definition)]
	public class AddNewOffersTest : IClassFixture<AddNewOffersTestFixture>
	{
		private FlatOffersDbContext _context;

		public AddNewOffersTest(FlatOffersDbContextFixture dbFixture)
		{
			_context = dbFixture.Context;
		}

		[Fact]
		public void There_Should_Be_One_FlattOffer_In_DB()
		{
			Assert.Equal(1, _context.FlatOffers.Count());
		}

		[Fact]
		public void Inserted_FlatOffer_Has_Expected_Values()
		{
			var ad = AdvertisementFactory.GetAdverstisementType1();

			Assert.Equal(1, _context.FlatOffers.Where(x =>
			   x.Address.Equals(ad.Address) &&
			   x.FlatSize == ad.FlatSize &&
			   x.FlatType == ad.FlatType &&
			   x.NumberOfRooms == ad.NumberOfRooms &&
			   x.Price == ad.Price &&
			   x.Removed == false)
			.Count());
		}

		[Fact]
		public void Inserted_Expected_Links()
		{
			var ad = AdvertisementFactory.GetAdverstisementType1();

			Assert.Equal(1, _context.FlatOffers
				.Where(x => x.Links.Any(y => y.Url.Equals(ad.Url) && y.UniqueId == ad.UniqueId))
				.Count());
		}

		[Fact]
		public void Inserted_Add_Notification()
		{
			var ad = AdvertisementFactory.GetAdverstisementType1();

			Assert.Equal(1, _context.FlatOffers
				.SelectMany(x => x.Notifications)
				.Where(x => x.Type == Entities.NotificationType.OfferAdded)
				.Count()
			);
		}
	}
}

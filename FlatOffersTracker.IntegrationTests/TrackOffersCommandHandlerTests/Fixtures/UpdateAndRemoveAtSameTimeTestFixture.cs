using Microsoft.EntityFrameworkCore;
using Respawn;
using System.Threading.Tasks;
using FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests.Factories;

namespace FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests.Fixtures
{
	public class UpdateAndRemoveAtSameTimeTestFixture
	{
		public const decimal UpdatedPrice = 444;

		public UpdateAndRemoveAtSameTimeTestFixture(FlatOffersDbContextFixture dbFixture)
		{
			var connectionString = dbFixture.Context.Database.GetDbConnection().ConnectionString;
			var checkpoint = new Checkpoint();
			Task.Run(() => checkpoint.Reset(connectionString)).Wait();

			var Offer = FlatOfferFactory.GetFlatOfferType1();
			var Ad1 = AdvertisementFactory.GetAdverstisementType1();
			var Ad2 = AdvertisementFactory.GetAdvertisementBasedOnOffer(Offer);
			Ad2.Price = UpdatedPrice;

			PopulateTables(dbFixture, Offer);

			var collector = new TestCollectorStub();
			collector.Add(Ad1);
			collector.Add(Ad2);

			var underTest = TrackOfferHandlerFactory.GetInstance(dbFixture.Context, collector);

			underTest.Execute();
		}

		private static void PopulateTables(FlatOffersDbContextFixture dbFixture, Entities.FlatOffer Offer)
		{
			var context = dbFixture.Context;

			context.FlatOffers.Add(Offer);
			context.SaveChanges();
		}
	}
}

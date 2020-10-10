using Microsoft.EntityFrameworkCore;
using Respawn;
using System.Threading.Tasks;
using FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests.Factories;
using FlatOffersTracker.IntegrationTests.BaseFixtures;

namespace FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests
{	
	public class RemoveAndAddFlatOfferTestFixture
	{
		public RemoveAndAddFlatOfferTestFixture(FlatOffersDbContextFixture dbFixture)
		{
			var connectionString = dbFixture.Context.Database.GetDbConnection().ConnectionString;
			var checkpoint = new Checkpoint();
			Task.Run(() => checkpoint.Reset(connectionString)).Wait();

			var Offer = FlatOfferFactory.GetFlatOfferType1();
			var Ad2 = AdvertisementFactory.GetAdverstisementType1();

			PopulateTables(dbFixture, Offer);

			var collector = new TestCollectorStub();
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

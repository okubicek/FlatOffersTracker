using Microsoft.EntityFrameworkCore;
using Respawn;
using System.Threading.Tasks;
using FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests.Factories;

namespace FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests.Fixtures
{
	public class AddNewOffersTestFixture
	{
		public AddNewOffersTestFixture(FlatOffersDbContextFixture dbFixture)
		{
			var connectionString = dbFixture.Context.Database.GetDbConnection().ConnectionString;
			var checkpoint = new Checkpoint();
			Task.Run(() => checkpoint.Reset(connectionString)).Wait();

			var Ad1 = AdvertisementFactory.GetAdverstisementType1();

			var collector = new TestCollectorStub();
			collector.Add(Ad1);

			var underTest = TrackOfferHandlerFactory.GetInstance(dbFixture.Context, collector);

			underTest.Execute();
		}
	}
}

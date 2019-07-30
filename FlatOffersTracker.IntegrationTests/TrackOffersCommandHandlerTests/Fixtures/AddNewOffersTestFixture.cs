using FlatOffersTracker.Parsing;
using FlatOffersTrackerBackgroundApp.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

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

			var underTest = new TractOffersCommandHandler(new FlatOffersRepository(dbFixture.Context),
				new UpdateOffersBasedOnAdvertisementsCommandHandler(),
				new List<IAdvertisementsCollector> { collector },
				Log.Logger);

			underTest.Execute();
		}
	}
}

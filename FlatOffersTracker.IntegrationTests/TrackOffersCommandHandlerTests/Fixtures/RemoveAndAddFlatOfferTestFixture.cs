using FlatOffersTracker.Parsing;
using FlatOffersTrackerBackgroundApp.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests
{	
	public class RemoveAndAddFlatOfferTestFixture : IDisposable
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

			var underTest = new TractOffersCommandHandler(new FlatOffersRepository(dbFixture.Context),
				new UpdateOffersBasedOnAdvertisementsCommandHandler(),
				new List<IAdvertisementsCollector> { collector },
				Log.Logger);

			underTest.Execute();
		}

		private static void PopulateTables(FlatOffersDbContextFixture dbFixture, Entities.FlatOffer Offer)
		{
			var context = dbFixture.Context;

			context.FlatOffers.Add(Offer);
			context.SaveChanges();
		}

		public void Dispose()
		{			
		}
	}
}

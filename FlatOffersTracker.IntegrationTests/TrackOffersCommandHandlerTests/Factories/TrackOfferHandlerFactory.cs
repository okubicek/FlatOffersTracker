using EFRepository.DataAccess.Context;
using EFRepository.DataAccess.Repositories;
using FlatOffersTracker.Cqrs.Commands;
using FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests.Stubs;
using FlatOffersTracker.Parsing;
using Serilog;
using System.Collections.Generic;

namespace FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests.Factories
{
	public class TrackOfferHandlerFactory
	{
		public static TractOffersHandler GetInstance(FlatOffersDbContext context, IAdvertisementsCollector collector)
		{
			var repo = new FlatOffersRepository(context);
			return new TractOffersHandler(repo,
				new UpdateOffersBasedOnAdvertisementsHandler(new GetImageByUrlStub()),
				new List<IAdvertisementsCollector> { collector },
				Log.Logger);
		}
	}
}

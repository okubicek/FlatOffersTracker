using EFRepository.DataAccess.Context;
using EFRepository.DataAccess.Repositories;
using FlatOffersTracker.Cqrs.Queries;
using FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests.Stubs;
using FlatOffersTracker.Parsing;
using Serilog;
using System.Collections.Generic;

namespace FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests.Factories
{
	public class TrackOfferHandlerFactory
	{
		public static TractOffersCommandHandler GetInstance(FlatOffersDbContext context, IAdvertisementsCollector collector)
		{
			var repo = new FlatOffersRepository(context);
			return new TractOffersCommandHandler(repo,
				new UpdateOffersBasedOnAdvertisementsCommandHandler(new GetImageByUrlStub(), new GetImagesPerOfferHandler(repo)),
				new List<IAdvertisementsCollector> { collector },
				Log.Logger);
		}
	}
}

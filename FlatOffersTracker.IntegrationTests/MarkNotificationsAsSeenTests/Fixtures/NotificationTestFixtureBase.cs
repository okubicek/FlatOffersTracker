using EFRepository.DataAccess.Context;
using EFRepository.DataAccess.Repositories;
using FlatOffersTracker.Cqrs.Commands;
using FlatOffersTracker.IntegrationTests.BaseFixtures;
using FlatOffersTracker.IntegrationTests.Helpers;
using System.Collections.Generic;

namespace FlatOffersTracker.IntegrationTests.MarkNotificationsAsSeenTests.Fixtures
{
	public class NotificationTestFixtureBase : INotificationSetup
	{
		public NotificationTestFixtureBase(FlatOffersDbContextFixture dbFixture)
		{
			DbContext = dbFixture.Context;
			Handler = new SetNotificationsAsSeenHandler(new NotificationRepository(dbFixture.Context));
			FlatOfferIds = new List<int>();
		}

		public FlatOffersDbContext DbContext { get; }

		public ISetNotificationsAsSeenHandler Handler { get; }

		public IList<int> FlatOfferIds { get; }
	}
}

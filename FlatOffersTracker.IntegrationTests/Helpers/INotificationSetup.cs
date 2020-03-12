using FlatOffersTracker.Cqrs.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlatOffersTracker.IntegrationTests.Helpers
{
	public interface INotificationSetup : IFlatOffersDbContext
	{
		ISetNotificationsAsSeenHandler Handler { get; }
		IList<int> FlatOfferIds { get; }
		//NotificationTestContext NotificationSetupInfo { get; set; }
	}
}

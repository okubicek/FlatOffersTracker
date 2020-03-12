using FlatOffersTracker.Entities;
using FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests;
using System;
using System.Collections.Generic;

namespace FlatOffersTracker.IntegrationTests.Helpers
{
	public static class NotificationSetupMethods
	{
		private static void AddFlatOfferToDb(INotificationSetup setup, Func<FlatOffer, IEnumerable<Notification>> getNotifications)
		{
			var offer = FlatOfferFactory.GetFlatOfferType1();

			offer.Notifications.AddRange(getNotifications(offer));
			setup.DbContext.FlatOffers.Add(offer);

			setup.DbContext.SaveChanges();

			setup.FlatOfferIds.Add(offer.Id.Value);
		}

		public static INotificationSetup AddFlatOfferWithMultipleNotViewedNotificationsToDb(this INotificationSetup setup)
		{
			Func<FlatOffer, IEnumerable<Notification>> func = (FlatOffer offer) =>
			{
				var notification = new Notification(offer, NotificationType.OfferAdded);
				var notification2 = new Notification(offer, NotificationType.PriceChanged);

				return new List<Notification> { notification, notification2 };
			};

			AddFlatOfferToDb(setup, func);
			return setup;
		}

		public static INotificationSetup AddFlatOfferWithNotViewedNotificationToDb(this INotificationSetup setup)
		{
			Func<FlatOffer, IEnumerable<Notification>> func = (FlatOffer offer) =>
			{
				var notification = new Notification(offer, NotificationType.OfferAdded);

				return new List<Notification> { notification };
			};

			AddFlatOfferToDb(setup, func);
			return setup;
		}

		public static INotificationSetup ExecuteTest(this INotificationSetup setup, IList<int> flatOfferIds)
		{
			setup.Handler.Execute(new Cqrs.Commands.SetNotificationsAsSeenCommand
			{
				flatOffersIds = flatOfferIds
			});

			return setup;
		}
	}
}


using FlatOffersTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatOffersTracker.Web.Helpers
{
	public static class NotificationResolver
	{
		public static string ResolveNotification(IEnumerable<Notification> notifications)
		{
			if (!notifications.Any())
			{
				return null;
			}

			return notifications.Count() == 1 ?
				notifications.First().Type.ToString() :
				ReturnNotificationByPriority(notifications).Type.ToString();
		}

		private static Notification ReturnNotificationByPriority(IEnumerable<Notification> notifications)
		{
			NotificationType notifType = GetNotificationTypeByPriority(notifications);

			return notifications.First(x => x.Type == notifType);
		}

		private static NotificationType GetNotificationTypeByPriority(IEnumerable<Notification> notifications)
		{
			if (notifications.Any(x => x.Type == NotificationType.OfferRemoved))
			{
				return NotificationType.OfferRemoved;
			}

			if (notifications.Any(x => x.Type == NotificationType.PriceChanged))
			{
				return NotificationType.OfferAdded;
			}

			if (notifications.Any(x => x.Type == NotificationType.OfferAdded))
			{
				return NotificationType.OfferAdded;
			}

			throw new NotImplementedException("Valid notification type not found");
		}
	}
}

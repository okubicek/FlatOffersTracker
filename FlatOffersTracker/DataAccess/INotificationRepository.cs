using FlatOffersTracker.Entities;
using System.Collections.Generic;

namespace FlatOffersTracker.DataAccess
{
	public interface INotificationRepository
	{
		IList<Notification> GetByFlatOffersIds(IList<int> flatOfferIds);

		void Save(IEnumerable<Notification> notifications);
	}
}

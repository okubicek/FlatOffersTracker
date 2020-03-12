using EFRepository.DataAccess.Context;
using FlatOffersTracker.DataAccess;
using FlatOffersTracker.Entities;
using System;
using System.Collections.Generic;
using EFRepository.DataAccess.Extensions;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EFRepository.DataAccess.Repositories
{
	public class NotificationRepository : INotificationRepository
	{
		private FlatOffersDbContext _dbContext;

		public NotificationRepository(FlatOffersDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IList<Notification> GetByFlatOffersIds(IList<int> flatOfferIds)
		{
			return _dbContext
				.FlatOffers
				.Include(x => x.Notifications)
				.Where(x => x.Id.HasValue && flatOfferIds.Contains(x.Id.Value))
				.SelectMany(x => x.Notifications)
				.ToList();				
		}

		public void Save(IEnumerable<Notification> notifications)
		{
			_dbContext.Notifications.UpdateRange(notifications);

			_dbContext.SaveChanges();
		}
	}
}

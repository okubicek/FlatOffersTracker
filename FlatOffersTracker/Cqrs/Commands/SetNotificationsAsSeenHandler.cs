using Common.Cqrs;
using FlatOffersTracker.DataAccess;

namespace FlatOffersTracker.Cqrs.Commands
{
	public interface ISetNotificationsAsSeenHandler : ICommand<SetNotificationsAsSeenCommand>
	{
	}

	public class SetNotificationsAsSeenHandler : ISetNotificationsAsSeenHandler
	{
		private INotificationRepository _notificationRepository;

		public void Execute(SetNotificationsAsSeenCommand command)
		{
			var notifications = _notificationRepository.GetByFlatOffersIds(command.flatOffersIds);

			foreach (var notification in notifications)
			{
				notification.MarkAsViewed();
			}

			_notificationRepository.Save(notifications);
		}
	}
}

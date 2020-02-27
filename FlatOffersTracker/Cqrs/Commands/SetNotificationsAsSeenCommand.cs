using System;
using System.Collections.Generic;
using System.Text;

namespace FlatOffersTracker.Cqrs.Commands
{
	public class SetNotificationsAsSeenCommand
	{
		public IList<int> flatOffersIds { get; set; }
	}
}

using FlatOffersTracker.Entities;
using System.Collections.Generic;

namespace FlatOffersTracker.Parsing
{
	public interface IAdvertisementsCollector
	{
		IEnumerable<Advertisement> Collect();
	}
}
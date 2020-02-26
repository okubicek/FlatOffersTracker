using FlatOffersTracker.Entities;
using System.Collections.Generic;

namespace FlatOffersTracker.Cqrs.Commands
{
	public class UpdateOffersBasedOnAdvertisementsCommand
	{
		public IEnumerable<FlatOffer> Offers { get; set; }

		public IEnumerable<Advertisement> Advertisements { get; set; }
	}
}

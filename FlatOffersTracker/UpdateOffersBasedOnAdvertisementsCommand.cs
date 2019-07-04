using FlatOffersTracker.Entities;
using System.Collections.Generic;

namespace FlatOffersTracker
{
	public class UpdateOffersBasedOnAdvertisementsCommand
	{
		public IEnumerable<FlatOffer> Offers { get; set; }

		public IEnumerable<Advertisement> Advertisements { get; set; }
	}
}

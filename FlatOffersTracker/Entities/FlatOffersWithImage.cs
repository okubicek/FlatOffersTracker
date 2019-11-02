using System;
using System.Collections.Generic;

namespace FlatOffersTracker.Entities
{
	public class FlatOffersWithImage
	{
		private HashSet<int> _flatOfferImageCounts;

		public FlatOffersWithImage(HashSet<int> counts)
		{
			_flatOfferImageCounts = counts ?? throw new ArgumentNullException();
		}

		public bool HasImages(int flatOfferId)
		{
			return _flatOfferImageCounts.Contains(flatOfferId);
		}
	}
}
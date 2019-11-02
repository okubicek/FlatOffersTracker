using FlatOffersTracker.Entities;
using System.Collections.Generic;
using System.Linq;

namespace FlatOffersTracker.Helpers
{
	public class FlatOffersToAdvertisementMatcher
	{
		public List<(FlatOffer offer, Advertisement ad)> Matched { get; private set; }

		public List<FlatOffer> UnmatchedOffers { get; private set; }

		public List<Advertisement> UnmatchedAds { get; private set; }

		public FlatOffersToAdvertisementMatcher(IEnumerable<FlatOffer> offers, 
			IEnumerable<Advertisement> ads)
		{
			Matched = new List<(FlatOffer offer, Advertisement ad)>();
			UnmatchedAds = new List<Advertisement>();
			UnmatchedOffers = new List<FlatOffer>();

			Match(offers, ads);
		}

		private void Match(IEnumerable<FlatOffer> offers, IEnumerable<Advertisement> ads)
		{
			var adDictionary = ads.ToDictionary(x => x.UniqueId, x => x);

			foreach (var offer in offers)
			{
				var isMatched = false;

				foreach (var link in offer.Links)
				{
					if (adDictionary.TryGetValue(link.UniqueId, out var ad))
					{
						isMatched = true;
						Matched.Add((offer, ad));
					}
				}

				if (!isMatched)
				{
					UnmatchedOffers.Add(offer);
				}
			}

			UnmatchedAds = ads.Except(Matched.Select(x => x.ad)).ToList();
		}
	}
}
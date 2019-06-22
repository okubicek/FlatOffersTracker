using FlatOffersTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatOffersTracker
{
	public class UpdateOffersBasedOnAdvertisementsCommandHandler
	{
		public IEnumerable<FlatOffer> Execute(IEnumerable<FlatOffer> offers, IEnumerable<Advertisement> advertisements)
		{
			var matchedAdvertisements = new List<Advertisement>();
			var offersWithMatchingAdvertisement = new HashSet<FlatOffer>();

			foreach (var offer in offers)
			{
				foreach (var ad in advertisements)
				{
					var matched = offer.TryMatchAdvertisement(ad);
					if (matched)
					{
						matchedAdvertisements.Add(ad);
						offersWithMatchingAdvertisement.AddIfNotExists(offer);
					}
				}
			}

			var closedOffers = offers.Except(offersWithMatchingAdvertisement);
			var unmatchedAdvertisements = advertisements.Except(matchedAdvertisements);

			ProcessUnmatchedAdvertisements(offersWithMatchingAdvertisement, unmatchedAdvertisements);

			return CombineOffers(offersWithMatchingAdvertisement, closedOffers);
		}

		private static void ProcessUnmatchedAdvertisements(HashSet<FlatOffer> offersWithMatchingAdvertisement, IEnumerable<Advertisement> unmatchedAdvertisements)
		{
			foreach (var ad in unmatchedAdvertisements)
			{
				var offer = new FlatOffer
				{
					DateAdded = DateTime.Now,
					FlatSize = ad.FlatSize,
					FlatType = ad.FlatType,
					NumberOfRooms = ad.NumberOfRooms,
					Address = ad.Address,
					Price = ad.Price
				};

				if (offersWithMatchingAdvertisement.TryGetValue(offer, out var offerToUpdate))
				{
					offer = offerToUpdate;
				}
				else
				{
					offersWithMatchingAdvertisement.Add(offer);
				}

				offer.Links.Add(new Link(ad.Url, offer));
			}
		}

		private static IEnumerable<FlatOffer> CombineOffers(HashSet<FlatOffer> offersWithMatchingAdvertisement, IEnumerable<FlatOffer> closedOffers)
		{
			var offers = offersWithMatchingAdvertisement.ToList();
			offers.AddRange(closedOffers);
			return offers;
		}
	}
}

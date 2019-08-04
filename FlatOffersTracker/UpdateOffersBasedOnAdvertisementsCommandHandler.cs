using Common.Cqrs;
using FlatOffersTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatOffersTracker
{
	public class UpdateOffersBasedOnAdvertisementsCommandHandler
		: ICommand<IEnumerable<FlatOffer>, UpdateOffersBasedOnAdvertisementsCommand>
	{
		public IEnumerable<FlatOffer> Execute(UpdateOffersBasedOnAdvertisementsCommand command)
		{
			MatchAdvertisementsToOffers(command.Offers,
				command.Advertisements,
				out var matchedAdvertisements,
				out var offersWithMatchingAdvertisement);

			var closedOffers = CloseUnmatchedOffers(command.Offers, offersWithMatchingAdvertisement);

			var unmatchedAdvertisements = command.Advertisements.Except(matchedAdvertisements);
			GenerateNewOffersForUnmatchedAdvertisements(offersWithMatchingAdvertisement, unmatchedAdvertisements);

			var result = ConcatenateOffers(offersWithMatchingAdvertisement, closedOffers);
			return result;
		}

		private static IEnumerable<FlatOffer> CloseUnmatchedOffers(IEnumerable<FlatOffer> offers, HashSet<FlatOffer> offersWithMatchingAdvertisement)
		{
			var closedOffers = offers.Except(offersWithMatchingAdvertisement);
			foreach (var closedOffer in closedOffers)
			{
				closedOffer.MarkAsRemoved();
			}

			return closedOffers;
		}

		private static void MatchAdvertisementsToOffers(IEnumerable<FlatOffer> offers,
			IEnumerable<Advertisement> advertisements,
			out List<Advertisement> matchedAdvertisements,
			out HashSet<FlatOffer> offersWithMatchingAdvertisement)
		{
			matchedAdvertisements = new List<Advertisement>();
			offersWithMatchingAdvertisement = new HashSet<FlatOffer>();

			foreach (var offer in offers)
			{
				foreach (var ad in advertisements)
				{
					var matched = offer.MatchAdvertisement(ad);
					if (matched)
					{
						if(offer.Price != ad.Price)
						{
							offer.Price = ad.Price;
							offer.AddNotification(NotificationType.PriceChanged);
						}
						matchedAdvertisements.Add(ad);
						offersWithMatchingAdvertisement.AddIfNotExists(offer);
					}
				}
			}
		}

		private static void GenerateNewOffersForUnmatchedAdvertisements(HashSet<FlatOffer> offersWithMatchingAdvertisement, IEnumerable<Advertisement> unmatchedAdvertisements)
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
					offer.AddNotification(NotificationType.OfferAdded);
					offersWithMatchingAdvertisement.Add(offer);
				}

				offer.AddLink(ad.Url, ad.UniqueId);
			}
		}

		private static IEnumerable<FlatOffer> ConcatenateOffers(HashSet<FlatOffer> offersWithMatchingAdvertisement, IEnumerable<FlatOffer> closedOffers)
		{
			var offers = offersWithMatchingAdvertisement.ToList();
			offers.AddRange(closedOffers);

			return offers;
		}
	}
}
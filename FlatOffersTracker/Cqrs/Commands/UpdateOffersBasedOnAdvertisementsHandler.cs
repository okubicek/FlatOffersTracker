using Common.Cqrs;
using FlatOffersTracker.Cqrs.Queries;
using FlatOffersTracker.Entities;
using FlatOffersTracker.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatOffersTracker.Cqrs.Commands
{
	public interface IUpdateOffersBasedOnAdvertisementsHandler 
		: ICommand<IEnumerable<FlatOffer>, UpdateOffersBasedOnAdvertisementsCommand>
	{
	}

	public class UpdateOffersBasedOnAdvertisementsHandler
		: IUpdateOffersBasedOnAdvertisementsHandler
	{
		private IGetImagesByUrlHandler _getImages;

		public UpdateOffersBasedOnAdvertisementsHandler(
			IGetImagesByUrlHandler getImages)
		{
			_getImages = getImages;
		}

		public IEnumerable<FlatOffer> Execute(UpdateOffersBasedOnAdvertisementsCommand command)
		{
			var matches = new FlatOffersToAdvertisementMatcher(command.Offers, command.Advertisements);
			ProcessOffersMatchedToAds(matches.Matched);

			var closedOffers = CloseUnmatchedOffers(matches.UnmatchedOffers);
			var newOffers = GenerateNewOffersForUnmatchedAdvertisements(matches.UnmatchedAds);

			var result = ConcatenateOffers(matches.Matched, newOffers, closedOffers);
			return result;
		}

		private void ProcessOffersMatchedToAds(List<(FlatOffer offer, Advertisement ad)> matches)
		{
			foreach (var match in matches)
			{
				var offer = match.offer;
				var ad = match.ad;

				if (match.offer.Price != ad.Price)
				{
					offer.Price = ad.Price;
					offer.AddNotification(NotificationType.PriceChanged);
				}
			}
		}

		private static IEnumerable<FlatOffer> CloseUnmatchedOffers(IEnumerable<FlatOffer> offersToClose)
		{
			foreach (var offerToClose in offersToClose)
			{
				offerToClose.MarkAsRemoved();
			}

			return offersToClose;
		}

		private IEnumerable<FlatOffer> GenerateNewOffersForUnmatchedAdvertisements(IEnumerable<Advertisement> unmatchedAdvertisements)
		{
			var newOffers = new HashSet<FlatOffer>(); // for cases when same offer appears on multiple sites

			var imageDownloadRequest = new GetImagesByUrlQuery
			{
				Requests = unmatchedAdvertisements.Select(x => (x.UniqueId, x.ImagesUrl))
			};

			var downloadedImages = _getImages.Get(imageDownloadRequest);

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

				if (newOffers.TryGetValue(offer, out var offerToUpdate))
				{
					offer = offerToUpdate;
				}
				else
				{
					offer.AddNotification(NotificationType.OfferAdded);
					FillOfferImages(ad.UniqueId, downloadedImages, offer);
					newOffers.Add(offer);
				}

				offer.AddLink(ad.Url, ad.UniqueId);
			}

			return newOffers.ToList();
		}

		private static IEnumerable<FlatOffer> ConcatenateOffers(List<(FlatOffer offer, Advertisement ad)> matched, IEnumerable<FlatOffer> newOffers, IEnumerable<FlatOffer> closedOffers)
		{
			var offers = matched.Select(x => x.offer).ToList();
			offers.AddRange(newOffers);
			offers.AddRange(closedOffers);

			return offers;
		}

		private static void FillOfferImages(long adId, Dictionary<long, List<byte[]>> downloadedImages, FlatOffer offer)
		{
			if (downloadedImages.TryGetValue(adId, out var images))
			{
				short order = 1;
				foreach (var image in images)
				{
					offer.AddImage(image, order);
					order++;
				}
			}
		}
	}
}
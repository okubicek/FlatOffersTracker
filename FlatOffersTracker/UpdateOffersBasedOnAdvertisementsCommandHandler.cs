using Common.Cqrs;
using FlatOffersTracker.Cqrs.Queries;
using FlatOffersTracker.Entities;
using FlatOffersTracker.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatOffersTracker
{
	public class UpdateOffersBasedOnAdvertisementsCommandHandler
		: ICommand<IEnumerable<FlatOffer>, UpdateOffersBasedOnAdvertisementsCommand>
	{
		private IQuery<Dictionary<long, List<byte[]>>, GetImagesByUrlQuery> _getImages;

		private IQuery<FlatOffersWithImage> _getFlatOffersWithImage;

		public UpdateOffersBasedOnAdvertisementsCommandHandler(
			IQuery<Dictionary<long, List<byte[]>>, GetImagesByUrlQuery> getImages, 
			IQuery<FlatOffersWithImage> getFlatOffersWithImage)
		{
			_getImages = getImages;
			_getFlatOffersWithImage = getFlatOffersWithImage;
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
			var offersWithImagesStored = _getFlatOffersWithImage.Get();

			var imageDownloadRequest = new GetImagesByUrlQuery
			{
				Requests = matches
					.Where(x => !offersWithImagesStored.HasImages(x.offer.Id.Value))
					.Select(x => (x.ad.UniqueId, x.ad.ImagesUrl))
			};

			var downloadedImages = _getImages.Get(imageDownloadRequest);

			foreach (var match in matches)
			{
				var offer = match.offer;
				var ad = match.ad;

				if (match.offer.Price != ad.Price)
				{
					offer.Price = ad.Price;
					offer.AddNotification(NotificationType.PriceChanged);
				}

				FillerOfferImages(match.ad.UniqueId, downloadedImages, offer);
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
					FillerOfferImages(ad.UniqueId, downloadedImages, offer);
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

		private static void FillerOfferImages(long adId, Dictionary<long, List<byte[]>> downloadedImages, FlatOffer offer)
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
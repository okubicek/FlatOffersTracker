using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatOffersTracker.Entities
{
	public class FlatOffer
	{
		public string Address { get; set; }

		public int FlatSize { get; set; }

		public FlatType FlatType { get; set; }

		public int NumberOfRooms { get; set; }

		public List<Link> Links { get; set; }

		public DateTime DateAdded { get; set; }

		public DateTime? DateRemoved { get; set; }

		public decimal Price { get; set; }

		public List<Notification> Notifications { get; set; }

		public bool Removed { get; set; }

		public void AddNotification(NotificationType notificationType)
		{
			if (Notifications == null)
			{
				Notifications = new List<Notification>();
			}

			Notifications.Add(new Notification(this, notificationType));
		}

		public void AddLink(string AdvertisementUrl, long uniqueId)
		{
			if (Links == null)
			{
				Links = new List<Link>();
			}

			Links.Add(new Link(AdvertisementUrl, uniqueId, this));
		}

		public void MarkAsRemoved()
		{
			Removed = true;
			DateRemoved = DateTime.Now;

			AddNotification(NotificationType.OfferRemoved);
		}

		public bool MatchAdvertisement(Advertisement ad)
		{
			if (MatchOnUniqueId(ad))
			{
				return true;
			}

			if (MatchOnFlatParams(ad))
			{
				AddLink(ad.Url, ad.UniqueId);
				return true;
			}

			return false;
		}

		public override bool Equals(object obj)
		{
			var offer = obj as FlatOffer;
			return offer != null &&
				   Address == offer.Address &&
				   FlatSize == offer.FlatSize &&
				   FlatType == offer.FlatType &&
				   NumberOfRooms == offer.NumberOfRooms;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Address, FlatSize, FlatType, NumberOfRooms);
		}

		private bool MatchOnFlatParams(Advertisement ad)
		{
			return FlatSize == ad.FlatSize &&
							FlatType == ad.FlatType &&
							NumberOfRooms == ad.NumberOfRooms &&
							Address.Equals(ad.Address);
		}

		private bool MatchOnUniqueId(Advertisement ad)
		{
			return Links.Any(lnk => lnk.UniqueId ==  ad.UniqueId);
		}
	}
}

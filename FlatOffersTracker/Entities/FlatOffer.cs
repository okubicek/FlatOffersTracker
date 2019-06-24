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

		public bool TryMatchAdvertisement(Advertisement ad)
		{
			if (MatchOnUrl(ad))
			{
				return true;
			}

			if (MatchOnFlatParams(ad))
			{
				Links.Add(new Link(ad.Url, this));
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

		private bool MatchOnUrl(Advertisement ad)
		{
			return Links.Any(lnk => lnk.Url.Equals(ad.Url, StringComparison.CurrentCultureIgnoreCase));
		}

	}
}

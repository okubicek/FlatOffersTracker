using System;
using System.Collections.Generic;

namespace FlatOffersTracker.Entities
{
	public class Advertisement
	{
		public string Url { get; set; }

		public long UniqueId { get; set; }

		public string Address { get; set; }

		public int FlatSize { get; set; }

		public FlatType FlatType { get; set; }

		public int NumberOfRooms { get; set; }

		public decimal Price { get; set; }

		public List<string> ImagesUrl { get; set;}

		public override bool Equals(object obj)
		{
			var advertisement = obj as Advertisement;
			return advertisement != null &&
				   UniqueId == advertisement.UniqueId;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(UniqueId);
		}
	}
}

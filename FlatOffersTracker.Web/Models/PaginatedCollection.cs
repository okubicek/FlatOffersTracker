using System.Collections.Generic;

namespace FlatOffersTracker.Web.Models
{
	public class PaginatedCollection<T>
	{
		public IEnumerable<T> Results { get; set; }

		public int PageNumber {get; set;}

		public int PageCount { get; set; }
	}
}
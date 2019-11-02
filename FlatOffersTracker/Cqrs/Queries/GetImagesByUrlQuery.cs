using System.Collections.Generic;

namespace FlatOffersTracker.Cqrs.Queries
{
	public class GetImagesByUrlQuery
	{
		public IEnumerable<(long Id, List<string> Urls)> Requests { get; set; }
	}
}

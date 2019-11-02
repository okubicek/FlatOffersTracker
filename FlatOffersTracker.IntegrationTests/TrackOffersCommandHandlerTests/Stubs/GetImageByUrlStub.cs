using Common.Cqrs;
using FlatOffersTracker.Cqrs.Queries;
using System.Collections.Generic;

namespace FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests.Stubs
{
	public class GetImageByUrlStub : IQuery<Dictionary<long, List<byte[]>>, GetImagesByUrlQuery>
	{
		public Dictionary<long, List<byte[]>> Get(GetImagesByUrlQuery query)
		{
			var dict = new Dictionary<long, List<byte[]>>();
			
			foreach(var request in query.Requests)
			{
				dict.Add(request.Id, new List<byte[]> { new byte[] { 0x20, 0x38, 0x45, 0x60, 0x81, 0x47, 0x28 } });
			}

			return dict;
		}
	}
}

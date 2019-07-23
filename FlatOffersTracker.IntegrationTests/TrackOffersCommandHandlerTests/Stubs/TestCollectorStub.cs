using FlatOffersTracker.Entities;
using FlatOffersTracker.Parsing;
using System.Collections.Generic;

namespace FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests
{
	class TestCollectorStub : IAdvertisementsCollector
	{
		private List<Advertisement> _ads = new List<Advertisement>();

		public IEnumerable<Advertisement> Collect()
		{
			return _ads;
		}

		public void Add(Advertisement ad)
		{
			_ads.Add(ad);
		}
	}
}

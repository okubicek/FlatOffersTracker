using FlatOffersTracker.Entities;
using FlatOffersTrackerBackgroundApp.DataAccess.Context;
using System.Linq;
using Xunit;

namespace FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests
{
	[Collection(FlatOffersDbContextFixtureDefinition.Definition)]
	public class RemoveAndAddAtSameTimeTest : IClassFixture<RemoveAndAddFlatOfferTestFixture>
	{
		private FlatOffersDbContext _context;

		public RemoveAndAddAtSameTimeTest(FlatOffersDbContextFixture dbFixture)
		{
			_context = dbFixture.Context;
		}

		[Fact]
		public void ThereShouldBeRemovedNotificationRaised()
		{
			var removed = _context.FlatOffers.Where(x => x.Removed);
			Assert.Collection(removed,
				x => Assert.Contains(x.Notifications, n => n.Type == NotificationType.OfferRemoved));
		}
	}
}
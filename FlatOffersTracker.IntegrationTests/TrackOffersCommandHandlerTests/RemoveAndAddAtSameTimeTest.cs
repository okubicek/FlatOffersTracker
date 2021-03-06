using FlatOffersTracker.Entities;
using EFRepository.DataAccess.Context;
using System.Linq;
using Xunit;
using FlatOffersTracker.IntegrationTests.BaseFixtures;

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
		public void ThereShouldBeSingleRemovedOffer()
		{
			Assert.Equal(1, _context.FlatOffers.Where(x => x.Removed).Count());
		}

		[Fact]
		public void ThereShouldBeRemovedNotificationRaised()
		{
			var removed = _context.FlatOffers.Where(x => x.Removed);
			Assert.Collection(removed,
				x => Assert.Contains(x.Notifications, n => n.Type == NotificationType.OfferRemoved));
		}

		[Fact]
		public void ThereShouldBe2FlatOffersInDb()
		{
			Assert.Equal(2, _context.FlatOffers.Count());
		}

		[Fact]
		public void ThereShouldBe3NotificationsInDb()
		{
			var offers = _context.FlatOffers.SelectMany(x => x.Notifications);
			Assert.Equal(3, offers.Count());
		}
	}
}
using FlatOffersTracker.IntegrationTests.Helpers;
using FlatOffersTracker.IntegrationTests.MarkNotificationsAsSeenTests.Fixtures;
using FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests;
using System.Linq;
using Xunit;
using static FlatOffersTracker.IntegrationTests.MarkNotificationsAsSeenTests.WhenMarkingMultipleNotificationsAsViewed;

namespace FlatOffersTracker.IntegrationTests.MarkNotificationsAsSeenTests
{
	[Collection(FlatOffersDbContextFixtureDefinition.Definition)]
	public class WhenMarkingMultipleNotificationsAsViewed : IClassFixture<TestContext>
	{
		private TestContext _context;
		public WhenMarkingMultipleNotificationsAsViewed(TestContext context)
		{
			_context = context;
		}

		public class TestContext : NotificationTestFixtureBase
		{
			public TestContext(FlatOffersDbContextFixture dbFixture) : base(dbFixture)
			{
				this.ResetDb();
				this.AddFlatOfferWithMultipleNotViewedNotificationsToDb()
					.AddFlatOfferWithMultipleNotViewedNotificationsToDb()
					.ExecuteTest(this.FlatOfferIds);
			}
		}

		[Fact]
		public void ThereShouldBeNoNotViewedNotification()
		{
			var notViewed = _context.DbContext.FlatOffers
				.Where(x => x.Notifications.Any(n => !n.Viewed));

			Assert.False(notViewed.Any());
		}

		[Fact]
		public void ShouldntBeCaseOfMultiplicationOfFlatOffersInDb()
		{
			Assert.Equal(2, _context.DbContext.FlatOffers.Count());
		}

		[Fact]
		public void ShouldNotCauseMultiplicationOfNotificationsInDb()
		{
			var notifications = _context.DbContext.FlatOffers
				.SelectMany(x => x.Notifications);

			Assert.Equal(6, notifications.Count());
		}
	}
}

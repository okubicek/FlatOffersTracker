using EFRepository.DataAccess.Context;
using FlatOffersTracker.IntegrationTests.Helpers;
using FlatOffersTracker.IntegrationTests.MarkNotificationsAsSeenTests.Fixtures;
using FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests;
using System.Linq;
using Xunit;
using static FlatOffersTracker.IntegrationTests.MarkNotificationsAsSeenTests.WhenMarkingNotificationWithSingleUnmarkedNotificationTest;

namespace FlatOffersTracker.IntegrationTests.MarkNotificationsAsSeenTests
{
	[Collection(FlatOffersDbContextFixtureDefinition.Definition)]
	public class WhenMarkingNotificationWithSingleUnmarkedNotificationTest : IClassFixture<TestContext>
	{
		private TestContext _context;
		public WhenMarkingNotificationWithSingleUnmarkedNotificationTest(TestContext context)
		{
			_context = context;
		}

		public class TestContext : NotificationTestFixtureBase
		{
			public TestContext(FlatOffersDbContextFixture dbFixture) : base(dbFixture)
			{
				this.ResetDb();
				this.AddFlatOfferWithNotViewedNotificationToDb()
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
			Assert.Single(_context.DbContext.FlatOffers);
		}

		[Fact]
		public void ShouldNotCauseMultiplicationOfNotificationsInDb()
		{
			var notifications = _context.DbContext.FlatOffers
				.SelectMany(x => x.Notifications);

			Assert.Equal(2, notifications.Count());
		}
	}
}

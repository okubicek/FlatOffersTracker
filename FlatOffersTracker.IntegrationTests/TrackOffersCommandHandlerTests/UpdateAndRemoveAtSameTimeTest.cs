using FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests.Fixtures;
using FlatOffersTrackerBackgroundApp.DataAccess.Context;
using System.Linq;
using Xunit;

namespace FlatOffersTracker.IntegrationTests.TrackOffersCommandHandlerTests
{
	[Collection(FlatOffersDbContextFixtureDefinition.Definition)]
	public class UpdateAndRemoveAtSameTimeTest : IClassFixture<UpdateAndRemoveAtSameTimeTestFixture>
	{
		private FlatOffersDbContext _context;

		public UpdateAndRemoveAtSameTimeTest(FlatOffersDbContextFixture dbFixture)
		{
			_context = dbFixture.Context;
		}

		[Fact]
		public void There_Are_Two_FlatOffers_In_DB()
		{
			Assert.Equal(2, _context.FlatOffers.Count());
		}

		[Fact]
		public void There_Are_Two_Links_In_Db()
		{
			Assert.Equal(2, _context.FlatOffers
				.SelectMany(x => x.Links)
				.Count());
		}

		[Fact]
		public void Price_Changed_Notification_Was_Raised()
		{
			Assert.Equal(1, _context.FlatOffers
				.SelectMany(x => x.Notifications)
				.Where(x => x.Type == Entities.NotificationType.PriceChanged)
				.Count()
			);
		}

		[Fact]
		public void Added_Notification_Is_There_Twice()
		{
			Assert.Equal(2, _context.FlatOffers
				.SelectMany(x => x.Notifications)
				.Where(x => x.Type == Entities.NotificationType.OfferAdded)
				.Count()
			);
		}
	}
}

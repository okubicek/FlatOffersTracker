using FlatOffersTracker.Entities;
using FlatOffersTracker.Parsing;
using FlatOffersTrackerBackgroundApp.DataAccess.Context;
using FlatOffersTrackerBackgroundApp.DataAccess.Repositories;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FlatOffersTracker.IntegrationTests
{
	public class TrackOffersTest
	{
		private FlatOffersDbContext _context;

		public TrackOffersTest()
		{
			_context = new DbContextBuilder()
				.CreateDbContext<FlatOffersDbContext>(opt => new FlatOffersDbContext(opt));

			SetupTestEntities();
		}

		protected FlatOffer Offer { get; private set; }
		protected Advertisement Ad1 { get; private set; }
		protected Advertisement Ad2 { get; private set; }

		private void SetupTestEntities()
		{
			Offer = new FlatOffer
			{
				Address = "Ble",
				FlatSize = 80,
				FlatType = FlatType.Brick,
				NumberOfRooms = 3,
				Price = 800000
			};
			Offer.Links = new List<Link> { new Link("http://ble.com", 496846, Offer) };
			Offer.Notifications = new List<Notification> { new Notification(Offer, NotificationType.OfferAdded) };

			Ad1 = new Advertisement
			{
				Address = Offer.Address,
				FlatSize = Offer.FlatSize,
				FlatType = Offer.FlatType,
				NumberOfRooms = Offer.NumberOfRooms,
				Price = Offer.Price,
				UniqueId = Offer.Links.First().UniqueId,
				Url = Offer.Links.First().Url
			};

			Ad2 = new Advertisement
			{
				Address = "Random Addres 123",
				FlatSize = 90,
				FlatType = FlatType.Panel,
				NumberOfRooms = 2,
				Price = 100000,
				UniqueId = 808080,
				Url = "https://Traaadlfsl.com"
			};
		}

		protected class TestCollector : IAdvertisementsCollector
		{
			public List<Advertisement> _ads = new List<Advertisement>();

			public IEnumerable<Advertisement> Collect()
			{
				return _ads;
			}

			public void Add(Advertisement ad)
			{
				_ads.Add(ad);
			}
		}

		[Fact]
		public void Test1()
		{
			_context.FlatOffers.Add(Offer);
			_context.SaveChanges();

			var collector = new TestCollector();
			collector.Add(Ad2);

			var underTest = new TractOffersCommandHandler(new FlatOffersRepository(_context), 
				new UpdateOffersBasedOnAdvertisementsCommandHandler(),
				new List<IAdvertisementsCollector> { collector }, 
				Log.Logger);

			try
			{
				underTest.Execute();

				var removed = _context.FlatOffers.Where(x => x.Removed);
				Assert.Collection(removed,
					x => Assert.Contains(x.Notifications, n => n.Type == NotificationType.OfferRemoved));
			}
			finally
			{
				_context.Database.EnsureDeleted();
			}
		}
	}
}
